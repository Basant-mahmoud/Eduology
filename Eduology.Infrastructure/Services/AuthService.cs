﻿using Eduology.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Eduology.Application.Services.Helper;
using Eduology.Application.Services.Interface;
using Eduology.Application.Interface;
using Microsoft.EntityFrameworkCore;
using Eduology.Infrastructure.Persistence;

namespace Eduology.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private readonly EduologyDBContext _context;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt, EduologyDBContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _context = context;
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            // Check if the OrganizationId is provided and valid
            if (model.OrganizationId > 0)
            {
                var organizationExists = await _context.Organizations
                    .AnyAsync(o => o.OrganizationID == model.OrganizationId);

                if (!organizationExists)
                {
                    return new AuthModel { Message = "Invalid OrganizationId provided." };
                }
            }

            // Check if the email is already registered
            if (await _userManager.FindByEmailAsync(model.Email) != null)
                return new AuthModel { Message = "Email is already registered!" };

            // Check if the username is already registered
            if (await _userManager.FindByNameAsync(model.Username) != null)
                return new AuthModel { Message = "Username is already registered!" };

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                Name = model.Name,
                OrganizationId = model.OrganizationId 
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                return new AuthModel { Message = errors };
            }

            if (!string.IsNullOrEmpty(model.Role))
            {
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthModel
            {
                OrganizationId=model.OrganizationId,
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { model.Role },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName
            };
        }

        public async Task<AuthModel> LoginAsync(LoginModel model)
        {
            var authModel = new AuthModel();
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }
            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            // Write the token to a string
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            // Set the token in the authModel
            authModel.Token = token;
            authModel.IsAuthenticated = true;
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();
            // Extract OrganizationId from the token claims
            var jwtHandler = new JwtSecurityTokenHandler();
            var readToken = jwtHandler.ReadJwtToken(token); // Use the token string
            var organizationIdClaim = readToken.Claims.FirstOrDefault(c => c.Type == "organizationId");
            authModel.OrganizationId = user.OrganizationId;
            if (organizationIdClaim != null)
            {
                authModel.OrganizationId = Convert.ToInt32(organizationIdClaim.Value);
            }
            else
            {
                // Handle case where organizationId claim is missing (optional)
                authModel.Message = "OrganizationId not found in token.";
                // You can choose to return or handle this scenario as per your application logic
            }

            return authModel;
        }


        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null || !await _roleManager.RoleExistsAsync(model.Role))
                return "Invalid user ID or Role";

            if (await _userManager.IsInRoleAsync(user, model.Role))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, model.Role);

            return result.Succeeded ? string.Empty : "Something went wrong";
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}

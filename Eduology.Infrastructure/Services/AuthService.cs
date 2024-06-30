using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Eduology.Application.Interface;
using Eduology.Application.Services.Helper;
using Eduology.Application.Services.Interface;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Eduology.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly JWT _jwt;
        private readonly IAuthRepository _authRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        public AuthService(IOptions<JWT> jwt, IAuthRepository authRepository, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
        {
            _jwt = jwt.Value;
            _authRepository = authRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;

        }

        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (model.OrganizationId > 0)
            {
                if (!await _authRepository.OrganizationExistsAsync(model.OrganizationId))
                {
                    return new AuthModel { Message = "Invalid OrganizationId provided." };
                }
            }

            if (await _authRepository.EmailExistsAsync(model.Email))
                return new AuthModel { Message = "Email is already registered!" };

            if (await _authRepository.UsernameExistsAsync(model.Username))
                return new AuthModel { Message = "Username is already registered!" };

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                Name = model.Name,
                OrganizationId = model.OrganizationId
            };

            var password = GeneratePassword();

            // Create JWT token
            var jwtSecurityToken = await CreateJwtToken(user);

            //try
            //{
            //    await _emailSender.SendEmailAsync(user.Email, "Registration Successful",
            //        $"You have successfully registered to Eduology LMS. Your Email is {model.Email} and Your password is: {password}");
            //}
            //catch (Exception ex)
            //{
            //    var cleanedMessage = "Failed to send registration email: Transaction failed.";
            //    return new AuthModel { Message = cleanedMessage };
            //}

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                return new AuthModel { Message = errors };
            }

            if (!string.IsNullOrEmpty(model.Role))
            {
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            return new AuthModel
            {
                OrganizationId = model.OrganizationId,
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { model.Role },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName,
                Password = password,
                name = model.Name,
            };
        }

        private string GeneratePassword()
        {
            var random = new Random();
            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string nonAlphanumeric = "!@#$%^&*";

            var passwordChars = new char[8]; // Assuming a minimum length of 8 characters

            // Ensure the password has at least one uppercase letter, one lowercase letter, one digit, and one non-alphanumeric character
            passwordChars[0] = uppercase[random.Next(uppercase.Length)];
            passwordChars[1] = lowercase[random.Next(lowercase.Length)];
            passwordChars[2] = digits[random.Next(digits.Length)];
            passwordChars[3] = nonAlphanumeric[random.Next(nonAlphanumeric.Length)];

            // Fill the remaining characters with random choices from all categories
            string allChars = uppercase + lowercase + digits + nonAlphanumeric;
            for (int i = 4; i < passwordChars.Length; i++)
            {
                passwordChars[i] = allChars[random.Next(allChars.Length)];
            }

            // Shuffle the characters to ensure the password is not predictable
            return new string(passwordChars.OrderBy(c => random.Next()).ToArray());
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

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.name = user.Name;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();
            authModel.OrganizationId = user.OrganizationId;

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
                new Claim("uid", user.Id),
                new Claim("organizationId", user.OrganizationId.ToString())
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
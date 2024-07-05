using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Repositories
{
    public class AuthRepository:IAuthRepository
    {
        private readonly EduologyDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthRepository(EduologyDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> OrganizationExistsAsync(int organizationId)
        {
            return await _context.Organizations.AnyAsync(o => o.OrganizationID == organizationId);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _userManager.FindByNameAsync(username) != null;
        }
        
    }
}

using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly EduologyDBContext _context;
        public OrganizationRepository(EduologyDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Organization>> GetAllAsync()
        {
            return await _context.Organizations.Include(o => o.Courses).Include(o => o.Users).ToListAsync();
        }
        public async Task<Organization> GetByIdAsync(int id)
        {
            return await _context.Organizations.FindAsync(id);
        }
        public async Task AddAsync(Organization organization)
        {
            await _context.Organizations.AddAsync(organization);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var organization = await _context.Organizations.FindAsync(id);
            if (organization != null)
            {
                _context.Organizations.Remove(organization);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Organizations.AnyAsync(e => e.OrganizationID == id);
        }
    }
}

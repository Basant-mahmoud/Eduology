using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<IEnumerable<Organization>> GetAllAsync();
        Task<Organization> GetByIdAsync(int id);
        Task<Organization> AddAsync(Organization organization);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}

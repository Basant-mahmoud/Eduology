using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Interfaces
{
    public interface IAssignmentRepository
    {
        public Task<Assignment> CreateAsync(Assignment assignment);
        public Task<Assignment> GetByIdAsync(int id);
        public Task<Assignment> GetByNameAsync(String name);
        public Task<Assignment> UpdateAsync(int id,Assignment assignment);
        public Task<bool> DeleteAsync(int id);

    }
}

using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Application.Interface
{
    public interface IAsignmentServices
    {
        public Task<AssignmentDto> CreateAsync(AssignmentDto assignment);
        public Task<Assignment> UpdateAsync(int id,AssignmentDto assignment);
        public Task<bool> DeleteAsync(int id);
        public Task<Assignment> GetByIdAsync(int id);
        public Task<Assignment> GetByNameAsync(string name);
        public Task<List<AssignmentDto>> GetAllAsync();

    }
}

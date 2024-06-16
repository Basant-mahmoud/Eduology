using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Interfaces
{
    public interface IAdminRepository
    {
        Task<IEnumerable<UserDto>> GetAllInstructorsAsync();
        Task<UserDto> GetInstructorByIdAsync(string id);
        Task<UserDto> GetInstructorByNameAsync(string Name);
        Task<UserDto> GetInstructorByUserNameAsync(string UserName);
        Task<bool> DeleteInstructorAsync(string id); 

    }
}

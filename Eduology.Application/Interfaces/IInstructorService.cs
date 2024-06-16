using Eduology.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Application.Interfaces
{
    public interface IInstructorService
    {
        Task<IEnumerable<UserDto>> GetAllInstructorsAsync();
        Task<UserDto> GetInstructorByIdAsync(string instructorId);
        Task<UserDto> GetInstructorByNameAsync(string instructorName);
        Task<UserDto> GetInstructorByUserNameAsync(string instructorUserName);
        Task<bool> DeleteInstructorAsync(string instructorId);

    }
}

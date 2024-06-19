using Eduology.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Application.Interface
{
    public interface IInstructorService
    {
        Task<IEnumerable<UserDto>> GetAllInstructorsAsync();
        Task<UserDto> GetInstructorByIdAsync(string id);
        Task<UserDto> GetInstructorByNameAsync(string name);
        Task<UserDto> GetInstructorByUserNameAsync(string userName);
        Task<bool> DeleteInstructorAsync(string id);
        Task<bool> UpdateInstructorAsync(string id, UserDto updateInstructorDto);
        Task<bool> RegisterToCourseAsync(string instructorId, string courseCode);
    }
}

using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Interfaces
{
    public interface IInstructorRepository
    {
        Task<IEnumerable<UserDto>> GetAllInstructorsAsync();
        Task<UserDto> GetInstructorByIdAsync(string id);
        Task<UserDto> GetInstructorByNameAsync(string Name);
        Task<UserDto> GetInstructorByUserNameAsync(string UserName);
        Task<bool> DeleteInstructorAsync(string id);
        Task<bool> UpdateInstructorAsync(string id, UserDto updateInstructorDto);
        Task<bool> RegisterToCourseAsync(string instructorId, string courseCode);
        Task<List<CourseUserDto>> GetAllCourseToSpecificInstructorAsync(string InstructorId);
      
    }
}

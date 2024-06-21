using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Interfaces
{
    public interface IStudentRepository
    {
        Task<UserDto> GetStudentByIdAsync(string StudentId);
        Task<IEnumerable<UserDto>> GetAllStudentsAsync();
        Task<bool> UpdateStudentAsync(string studentId, UserDto userDto);
        Task<bool> DeleteStudentAsync(string studentId);
        Task<bool> RegisterToCourseAsync(string studentId, string courseCode);
        Task<List<CourseUserDto>> GetAllCourseToSpecificStudentAsync(string StudentId);
    }
}

using Eduology.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Application.Services.Interface
{
    public interface IStudentService
    {
        Task<UserDto> GetStudentByIdAsync(string StudentId);
        Task<IEnumerable<UserDto>> GetAllStudentsAsync();
        Task<bool> UpdateStudentAsync(string studentId, UserDto userDto);
        Task<bool> DeleteStudentAsync(string studentId);
    }
}

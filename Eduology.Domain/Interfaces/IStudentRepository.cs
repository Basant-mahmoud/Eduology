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
        Task<UserDto> GetStudentByIdAsync(string userId);
        Task<IEnumerable<UserDto>> GetAllStudentsAsync();
        Task<bool> DeleteAsync(string studentId);
    }
}

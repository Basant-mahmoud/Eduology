using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Application.Interface
{
    public interface ICourseService
    {
        Task<Course> CreateAsync(CourseCreationDto courseDto);
        Task<bool> DeleteAsync(String id);
        Task<CourseDetailsDto> GetByIdAsync(String ID,String UserID);
        Task<CourseDetailsDto> GetByNameAsync(string name,string UserID,string CourseId);
        Task<IEnumerable<CourseDetailsDto>> GetAllAsync(string UserID, string CourseId);
        Task<bool> UpdateAsync(String id, CourseDto course);
    }
}

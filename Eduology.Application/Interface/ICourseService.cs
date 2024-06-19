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
        Task<Course> CreateAsync(CourseDto course);
        Task<bool> DeleteAsync(String id);
        Task<CourseDetailsDto> GetByIdAsync(String id);
        Task<CourseDetailsDto> GetByNameAsync(string name);
        Task<IEnumerable<CourseDetailsDto>> GetAllAsync();
        Task<bool> UpdateAsync(String id, CourseDto course);
       
    }
}

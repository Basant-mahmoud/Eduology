using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course> CreateAsync(CourseDto course);
        Task<Course> DeleteAsync(String id);
        Task<CourseDetailsDto> GetByIdAsync(String id);
        Task<CourseDetailsDto> GetByNameAsync(string name);
        Task<IEnumerable<CourseDetailsDto>> GetAllAsync();
        Task<Course> UpdateAsync(String id,CourseDto course);
        Task<bool> AddInstructorToCourseAsync(string instructorId, string courseCode);
    }
}

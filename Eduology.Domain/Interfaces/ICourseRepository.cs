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
        Task Createsync(Course course);
        Task<bool> DeleteAsync(int id);
        Task<CourseDto> GetByIdAsync(int id);
        Task<CourseDto> GetByNameAsync(string name);
        Task<IEnumerable<CourseDto>> GetAllAsync();
        Task<bool> UpdateAsync(CourseDto course);
            
    }
}

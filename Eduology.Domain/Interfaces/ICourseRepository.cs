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
        Task CreateAsync(CourseDto course);
        Task<bool> DeleteAsync(int id);
        Task<CourseDetailsDto> GetByIdAsync(int id);
        Task<CourseDetailsDto> GetByNameAsync(string name);
        Task<IEnumerable<CourseDetailsDto>> GetAllAsync();
        Task<bool> UpdateAsync(int id,CourseDto course);
            
    }
}

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
        Task<courseCreationDetailsDto> CreateAsync(CourseDto courseDto,string adminId);
        Task<bool> DeleteAsync(String id);
        Task<CourseDetailsDto> GetByIdAsync(String ID,String UserID,string role);
        Task<CourseDetailsDto> GetByNameAsync(string name,string UserID,string role);
        Task<IEnumerable<CourseDetailsDto>> GetAllAsync(string UserID,string role);
        Task<bool> UpdateAsync(String id, CourseDto course);
        Task<List<Course>> GetAllByOrganizationIdAsync(int organizationId);
        Task<CourseDetailsDto> GetByIdForAdminAsync(string courseId,string adminId);
    }
}

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
        Task<Course> CreateAsync(Course course,string adminId);
        Task<bool> ExistsByCourseCodeAsync(string courseCode);
        Task<bool> OrganizationExistsAsync(int organizationId);
        Task<bool> DeleteAsync(String id);
        Task<CourseDetailsDto> GetByIdAsync(String id);
        Task<CourseDetailsDto> GetByNameAsync(string name);
        Task<IEnumerable<Course>> GetAllAsync(string userId,string role);
        Task<Course> UpdateAsync(String id, CourseDto course);
        Task<bool> IsInstructorAssignedToCourse(string instructorId, string courseId);
        Task<bool> ISStudentAssignedToCourse(string studentId, string courseId);
        Task<bool> IsInstructorAssignedToCourseByName(string instructorId, string Name);
        Task<bool> IStudentAssignedToCourseByName(string studentId, string Name);
        Task<bool> IsUserAssignedToCourseAsync(string userId, string courseId, string role);
        Task<bool> IsUserAssignedToCourseAsyncByNmae(string userId, string name, string role);
        Task<List<Course>> GetAllByOrganizationIdAsync(int organizationId);
        Task<Course> GetByIdForAdminAsync(string courseId,string adminId);
        Task<ApplicationUser> isAdminExistAsync(string AdminId);

    }
}

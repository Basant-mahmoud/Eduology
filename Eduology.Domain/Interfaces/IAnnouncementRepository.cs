using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Domain.Interfaces
{
    public interface IAnnouncementRepository
    {
        Task<Announcement> GetByIdAsync(int id);
        Task<Announcement> AddAsync(Announcement announcement);
        Task<bool> CourseExistsAsync(string courseId);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Announcement>> GetByCourseIdAsync(string courseId);
        Task<IEnumerable<Announcement>> GetAllAnnouncementsForStudentAsync(string studentId);
        Task<IEnumerable<Announcement>> GetAllAnnouncementsForInstructorAsync(string instructorId);

    }
}

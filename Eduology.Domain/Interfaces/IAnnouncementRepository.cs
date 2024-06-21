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
        Task<IEnumerable<Announcement>> GetAllAsync();
        Task<Announcement> GetByIdAsync(int id);
        Task<Announcement> AddAsync(Announcement announcement);
        Task<bool> CourseExistsAsync(string courseId);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Announcement>> GetByCourseIdAsync(string courseId);
        Task<Announcement> GetAnnouncementByIdAndCourseIdAsync(string courseId, int announcementId);
        Task<IEnumerable<Announcement>> GetAllAnnouncementsForStudentAsync(string studentId);

    }
}

using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly EduologyDBContext _context;

        public AnnouncementRepository(EduologyDBContext context)
        {
            _context = context;
        }

        public async Task<Announcement> GetByIdAsync(int id)
        {
            return await _context.Announcements.FindAsync(id);
        }

        public async Task<Announcement> AddAsync(Announcement announcement)
        {
            var instructorExists = await _context.Users.AnyAsync(u => u.Id == announcement.InstructorId);
            if (!instructorExists)
            {
                return null;
            }
            var courseExists = await _context.Courses.AnyAsync(c => c.CourseId == announcement.CourseId);
            if (!courseExists)
            {
                return null;
            }
            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();
            return announcement;
        }
        public async Task<bool> CourseExistsAsync(string courseId)
        {
            return await _context.Courses.AnyAsync(c => c.CourseId == courseId);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return false;
            }
            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Announcement>> GetByCourseIdAsync(string courseId)
        {
            return await _context.Announcements
                .Where(a => a.CourseId == courseId)
                .ToListAsync();
        }
        public async Task<Announcement> GetAnnouncementByIdAndCourseIdAsync(string courseId, int announcementId)
        {
            return await _context.Announcements
                                 .FirstOrDefaultAsync(a => a.CourseId == courseId && a.AnnouncementId == announcementId);
        }
        
            public async Task<IEnumerable<Announcement>> GetAllAnnouncementsForStudentAsync(string studentId)
            {
                return await _context.Announcements
                    .Where(a => a.Course.StudentCourses.Any(sc => sc.StudentId == studentId))
                    .Include(a => a.Course)
                    .Include(a => a.Instructor)
                    .ToListAsync();
            }
        

    }
}


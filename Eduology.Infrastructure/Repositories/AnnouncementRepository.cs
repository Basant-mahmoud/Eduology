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

        public async Task<IEnumerable<Announcement>> GetAllAsync()
        {
            return await _context.Announcements.ToListAsync();
        }

        public async Task<Announcement> GetByIdAsync(int id)
        {
            return await _context.Announcements.FindAsync(id);
        }

        public async Task<Announcement> AddAsync(Announcement announcement)
        {
            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();
            return announcement;
        }

        public async Task DeleteAsync(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement != null)
            {
                _context.Announcements.Remove(announcement);
                await _context.SaveChangesAsync();
            }
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
    }
}


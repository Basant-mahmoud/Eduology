using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IAnnouncementRepository _announcementRepository;

        public AnnouncementService(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public async Task<AnnouncementDto> CreateAsync(AnnouncementDto announcementDto, string courseId,string instructorId)
        {
            var announcement = new Announcement
            {
                Title = announcementDto.Title,
                Content = announcementDto.Content,
                CreatedAT = DateTime.UtcNow,
                CourseId = courseId,
                InstructorId = instructorId
            };

            var createdAnnouncement = await _announcementRepository.AddAsync(announcement);
            return ConvertToDto(createdAnnouncement);
        }
        public async Task<IEnumerable<AnnouncementDto>> GetAllAsync()
        {
            var announcements = await _announcementRepository.GetAllAsync();
            return announcements.Select(ConvertToDto);
        }

        public async Task<AnnouncementDto> GetByIdAsync(int id)
        {
            var announcement = await _announcementRepository.GetByIdAsync(id);
            return ConvertToDto(announcement);
        }

        public async Task DeleteAsync(int id)
        {
            await _announcementRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<AnnouncementDto>> GetAnnouncementsByCourseIdAsync(string courseId)
        {
            var announcements = await _announcementRepository.GetByCourseIdAsync(courseId);
            return announcements.Select(ConvertToDto);
        }

        public async Task<AnnouncementDto> GetAnnouncementByIdAndCourseIdAsync(string courseId, int announcementId)
        {
            var announcements = await _announcementRepository.GetByCourseIdAsync(courseId);
            var announcement = announcements.FirstOrDefault(a => a.AnnouncementId == announcementId);
            return ConvertToDto(announcement);
        }
        private AnnouncementDto ConvertToDto(Announcement announcement)
        {
            if (announcement == null)
                return null;

            return new AnnouncementDto
            {
                Id = announcement.AnnouncementId,
                Title = announcement.Title,
                Content = announcement.Content,
                CreatedAT = announcement.CreatedAT
            };
        }
    }
}

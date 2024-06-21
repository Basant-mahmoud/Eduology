using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnnouncementService(IAnnouncementRepository announcementRepository, UserManager<ApplicationUser> userManager)
        {
            _announcementRepository = announcementRepository;
            _userManager = userManager;
        }

        public async Task<AnnouncementDto> CreateAsync(AnnouncementDto announcementDto)
        {
            var instructor = await _userManager.FindByIdAsync(announcementDto.InstructorId);
            if (instructor == null)
            {
                return null;
            }

             var courseExists = await _announcementRepository.CourseExistsAsync(announcementDto.CourseId);
            if (!courseExists)
            {
                return null;
            }

            var announcement = new Announcement
            {
                Title = announcementDto.Title,
                Content = announcementDto.Content,
                CreatedAT = DateTime.UtcNow,
                CourseId = announcementDto.CourseId,
                InstructorId = announcementDto.InstructorId
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

        public async Task<bool> DeleteAsync(int id)
        {
            var announcementToDelete = await _announcementRepository.GetByIdAsync(id);
            if (announcementToDelete == null)
            {
                return false; 
            }

            await _announcementRepository.DeleteAsync(id);
            return true; 
        }
        public async Task<IEnumerable<AnnouncementDto>> GetAnnouncementsByCourseIdAsync(string courseId)
        {
            var announcements = await _announcementRepository.GetByCourseIdAsync(courseId);
            return announcements.Select(ConvertToDto); 
        }

        public async Task<AnnouncementDto> GetAnnouncementByIdAndCourseIdAsync(string courseId, int announcementId)
        {
            var announcement = await _announcementRepository.GetAnnouncementByIdAndCourseIdAsync(courseId, announcementId);
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
                CreatedAt = announcement.CreatedAT,
                CourseId = announcement.CourseId,
                InstructorId = announcement.InstructorId
            };
        }
    }
}

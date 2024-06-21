using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
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
        private readonly IStudentRepository _studentRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public AnnouncementService(IAnnouncementRepository announcementRepository, IStudentRepository studentRepository, UserManager<ApplicationUser> userManager)
        {
            _announcementRepository = announcementRepository;
            _studentRepository = studentRepository;
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

       public async Task<IEnumerable<AllAnnoncemetDto>> GetAllAnnouncementsForStudentAsync(string studentid)
        {
            if (string.IsNullOrEmpty(studentid))
            {
                throw new ArgumentException("Student ID not found or cannot be null .");
            }
            var isjoin= await _studentRepository.GetStudentByIdAsync(studentid);
            if (isjoin == null)
            {
                throw new ArgumentException("Student ID not exist");
            }
            var announcements = await _announcementRepository.GetAllAnnouncementsForStudentAsync(studentid);
            if (announcements == null || !announcements.Any())
            {
                return new List<AllAnnoncemetDto>();
            }
            return announcements.Select(a => new AllAnnoncemetDto
            {
                coursename = a.Course.Name,
                instructorname = a.Instructor.Name, 
                Content = a.Content,
                CreatedAT = a.CreatedAT
            }).ToList();
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

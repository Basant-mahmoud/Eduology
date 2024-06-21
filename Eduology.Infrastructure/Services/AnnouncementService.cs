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
        private readonly IStudentRepository _studentRepository;
        public AnnouncementService(IAnnouncementRepository announcementRepository, IStudentRepository _studentRepository)
        {
            _announcementRepository = announcementRepository;
            _studentRepository = _studentRepository;
        }

        public async Task<AnnouncementDto> CreateAsync(AnnouncementDto announcementDto)
        {
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
                CreatedAt = announcement.CreatedAT,
                CourseId = announcement.CourseId,
                InstructorId = announcement.InstructorId
            };
        }
       public async Task<IEnumerable<AllAnnoncemetDto>> GetAllAnnouncementsForStudentAsync(string studentid)
        {
            if (!string.IsNullOrEmpty(studentid))
            {
                throw new ArgumentException("Student ID not found or cannot be null .");
            }
            var isjoin= _studentRepository.GetStudentByIdAsync(studentid);
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

        
    }
}

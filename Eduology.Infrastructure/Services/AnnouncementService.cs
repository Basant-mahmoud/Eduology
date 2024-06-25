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
        private readonly ICourseRepository _courseRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public AnnouncementService(IAnnouncementRepository announcementRepository, IStudentRepository studentRepository, UserManager<ApplicationUser> userManager, ICourseRepository courseRepository)
        {
            _announcementRepository = announcementRepository;
            _studentRepository = studentRepository;
            _userManager = userManager;
            _courseRepository = courseRepository;


        }

        public async Task<AnnouncementDto> CreateAsync(string instructorid,CreateAnnoncementDto createannouncementDto)
        {
            var instructor = await _userManager.FindByIdAsync(instructorid);
            var isjointocourse=await _courseRepository.IsInstructorAssignedToCourse(instructorid, createannouncementDto.CourseId);
            var courseExists = await _announcementRepository.CourseExistsAsync(createannouncementDto.CourseId);
            if (!courseExists)
            {
                return null;
            }
            if (instructor == null)
            {
                return null;
            }
            if (isjointocourse == null)
            {
                return null;
            }
            

            var announcement = new Announcement
            {
                Title = createannouncementDto.Title,
                Content = createannouncementDto.Content,
                CreatedAT = createannouncementDto.CreatedAt ,
                CourseId = createannouncementDto.CourseId,
                InstructorId = instructorid
            };
            var createdAnnouncement = await _announcementRepository.AddAsync(announcement);
            return ConvertToDto(createdAnnouncement);
        }

        public async Task<AnnouncementDto> GetByIdAsync(string instructorid,int announcementid,string courseid)
        {
            var instructor = await _userManager.FindByIdAsync(instructorid);
            var courseExists = await _announcementRepository.CourseExistsAsync(courseid);
            if (!courseExists)
            {
                return null;
            }
            var isjointocourse = await _courseRepository.IsInstructorAssignedToCourse(instructorid, courseid);
            if (instructor == null)
            {
                return null;
            }
            if (isjointocourse == null)
            {
                return null;
            }
           
            var announcement = await _announcementRepository.GetByIdAsync(announcementid);
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
        public async Task<IEnumerable<AnnouncementDto>> GetAnnouncementsToInstructorByCourseIdAsync(string instructorId,string courseId)
        {
            var instructor = await _userManager.FindByIdAsync(instructorId);
            var courseExists = await _announcementRepository.CourseExistsAsync(courseId);
            if (!courseExists)
            {
                return null;
            }
            var isjointocourse = await _courseRepository.IsInstructorAssignedToCourse(instructorId, courseId);
            if (instructor == null)
            {
                return null;
            }
            if (isjointocourse == null)
            {
                return null;
            }
            var announcements = await _announcementRepository.GetByCourseIdAsync(courseId);
            if (announcements == null || !announcements.Any())
            {
                return new List<AnnouncementDto>();
            }
            return announcements.Select(ConvertToDto);
        }
        public async Task<IEnumerable<AnnouncementDto>> GetAnnouncementsToByStudentCourseIdAsync(string studentid, string courseId)
        {
            var instructor = await _userManager.FindByIdAsync(studentid);
            var courseExists = await _announcementRepository.CourseExistsAsync(courseId);
            if (!courseExists)
            {
                return null;
            }
            var isjointocourse = await _courseRepository.ISStudentAssignedToCourse(studentid, courseId);
            if (instructor == null)
            {
                return null;
            }
            if (isjointocourse == null)
            {
                return null;
            }
            var announcements = await _announcementRepository.GetByCourseIdAsync(courseId);
            if (announcements == null || !announcements.Any())
            {
                return new List<AnnouncementDto>();
            }
            return announcements.Select(ConvertToDto);
        }


       public async Task<IEnumerable<AllAnnoncemetDto>> GetAllAnnouncementsForStudentAsync(string studentid)
        {
            if (string.IsNullOrEmpty(studentid))
            {
                return null;
            }
            var isjoin= await _studentRepository.GetStudentByIdAsync(studentid);
            if (isjoin == null)
            {
                return null;
            }
            var announcements = await _announcementRepository.GetAllAnnouncementsForStudentAsync(studentid);
            if (announcements == null || !announcements.Any())
            {
                return new List<AllAnnoncemetDto>();
            }
            return announcements.Select(a => new AllAnnoncemetDto
            {
                coursename = a.Course.courseName,
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

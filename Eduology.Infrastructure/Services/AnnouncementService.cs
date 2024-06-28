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
using System.ComponentModel.DataAnnotations;

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
                throw new Exception($"Course with ID '{createannouncementDto.CourseId}' not found.");
            }
            if (instructor == null)
            {
                throw new Exception($"Instructor with ID '{instructorid}' not found.");
            }
            if (isjointocourse == null)
            {
                throw new Exception($"Instructor with ID '{instructorid}' is not assigned to course '{createannouncementDto.CourseId}'.");
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
                throw new Exception($"Course with ID '{courseid}' not found.");
            }
            var isjointocourse = await _courseRepository.IsInstructorAssignedToCourse(instructorid, courseid);
            if (instructor == null)
            {
                throw new Exception($"Instructor with ID '{instructorid}' not found.");
            }
            if (isjointocourse == null)
            {
                throw new Exception($"Instructor with ID '{instructorid}' is not assigned to course '{courseid}'.");
            }
           
            var announcement = await _announcementRepository.GetByIdAsync(announcementid);
            if (announcement == null)
            {
                throw new Exception($"Announcement with ID '{announcementid}' not found.");
            }
            return ConvertToDto(announcement);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var announcementToDelete = await _announcementRepository.GetByIdAsync(id);
            if (announcementToDelete == null)
            {
                throw new Exception($"Announcement with ID '{id}' not found.");
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
                throw new Exception($"Course with ID '{courseId}' not found.");
            }
            var isjointocourse = await _courseRepository.IsInstructorAssignedToCourse(instructorId, courseId);
            if (instructor == null)
            {
                throw new Exception($"Instructor with ID '{instructorId}' not found.");
            }
            if (isjointocourse == null)
            {
                throw new Exception($"Instructor with ID '{instructorId}' is not assigned to course '{courseId}'.");
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
            var student = await _userManager.FindByIdAsync(studentid);
            var courseExists = await _announcementRepository.CourseExistsAsync(courseId);
            if (!courseExists)
            {
                throw new Exception($"Course with ID '{courseId}' not found.");
            }
            var isjointocourse = await _courseRepository.ISStudentAssignedToCourse(studentid, courseId);
            if (student == null)
            {
                throw new Exception($"Student with ID '{studentid}' not found.");
            }
            if (isjointocourse == null)
            {
                throw new Exception($"Student with ID '{studentid}' is not assigned to course '{courseId}'.");
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
                throw new Exception("Student ID is required.");
            }
            var student= await _studentRepository.GetStudentByIdAsync(studentid);
            if (student == null)
            {
                throw new Exception($"Student with ID '{studentid}' not found.");
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
                CreatedAt = a.CreatedAT
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

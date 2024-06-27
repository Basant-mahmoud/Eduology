﻿using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduology.Infrastructure.Repositories;
using Eduology.Application.Services.Interface;
using System.Data;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
namespace Eduology.Infrastructure.Services
{

    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IInstructorService _instructorService;
        public CourseService(ICourseRepository courseRepository,IInstructorService instructorService,IStudentService studentService)
        {
            _courseRepository = courseRepository;
            _instructorService = instructorService;
        }

        public async Task<courseCreationDetailsDto> CreateAsync(CourseDto courseDto,string adminId)
        {
            // Generate a unique course code
            string courseCode;
            do
            {
                courseCode = GenerateCourseCode();
            } while (await _courseRepository.ExistsByCourseCodeAsync(courseCode));

            Course course = new Course
            {
                id = Guid.NewGuid().ToString(),
                Name = courseDto.Name,
                CourseCode = courseCode,
                Year = DateTime.Now.Year,
                OrganizationID = courseDto.OrganizationId,
                
            };
            try
            {
                await _courseRepository.CreateAsync(course, adminId);
                courseCreationDetailsDto details = new courseCreationDetailsDto
                {
                    CourseCode = courseCode,
                    Id = course.id,
                };
                return details;
            }
            catch (Exception ex) {
                throw new Exception("An error occurred while Creating the course " + ex.Message);
            }

        }

        private string GenerateCourseCode()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public async Task<IEnumerable<CourseDetailsDto>> GetAllAsync(string userID,string role)
        {
            var courses = await _courseRepository.GetAllAsync(userID, role);

            if (courses == null || !courses.Any())
                return Enumerable.Empty<CourseDetailsDto>();
            var courseDetails = courses.Select(c => new CourseDetailsDto
            {
                CourseId = c.id,
                CourseName = c.Name,
                CourseCode = c.CourseCode,
                Instructors = c.CourseInstructors.Select(ci => ci.Instructor.Name).ToList() ?? new List<string>(),
                students = c.StudentCourses.Select(sc => sc.Student.Name).ToList() ?? new List<string>(),
                assignments = c.Assignments.Select(a => new AssignmentDto
                {
                    Id = a.AssignmentId,
                    Description = a.Description,
                    Title = a.Title,
                    Deadline = a.Deadline,
                    CourseId = a.CourseId,
                    
                    fileURLs = a.File == null ? null : new AssignmentFileDto
                    {
                        URL = a.File.URL,
                        Title = a.File.Title
                    },
                }).ToList()?? new List<AssignmentDto> ()
            }).ToList();

            return courseDetails;
        }

        public async Task<bool> UpdateAsync(String id, CourseDto course)
        {
            var updatedCourse = await _courseRepository.UpdateAsync(id, course);
            return updatedCourse != null;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
                return false;
            await _courseRepository.DeleteAsync(id);
            return true;
        }

        public async Task<CourseDetailsDto> GetByIdAsync(string ID, string UserID,string role)
        {
            bool isEnrolledStudent = await _courseRepository.ISStudentAssignedToCourse(UserID, ID);
            bool isEnrolledInstructor = await _courseRepository.IsInstructorAssignedToCourse(UserID, ID);

            if (!isEnrolledStudent && !isEnrolledInstructor)
            {
                throw new Exception("You not registered in course");
            }
            var course = await _courseRepository.GetByIdAsync(ID);
            if (course == null)
                throw new Exception("Course Not found");
            return course;
        }

        public async Task<CourseDetailsDto> GetByNameAsync(string name, string UserID,string role)
        {
            bool IsRegisterd = await _courseRepository.IsUserAssignedToCourseAsyncByNmae(UserID,name, role);
            if (!IsRegisterd)
            {
                throw new Exception("You not registered in course");
            }
            var course = await _courseRepository.GetByNameAsync(name);
            if (course == null)
                throw new Exception("Course Not found");
            return course;
        }

        public async Task<List<Course>> GetAllByOrganizationIdAsync(int organizationId)
        {
            bool isExist = await _courseRepository.OrganizationExistsAsync(organizationId);
            if (!isExist)
                 throw new Exception("Organization not found") ;
            var courses = await _courseRepository.GetAllByOrganizationIdAsync(organizationId);
            return courses;
        }
        public async Task<CourseDetailsDto> GetByIdForAdminAsync(string courseId,string adminId)
        {
            try
            {
                var course = await _courseRepository.GetByIdForAdminAsync(courseId, adminId);
                if (course == null)
                    throw new Exception("Course not found");
                return new CourseDetailsDto
                {
                    CourseId = courseId,
                    assignments = course.Assignments.Select(a => new AssignmentDto
                    {
                        Id = a.AssignmentId,
                        CourseId = a.CourseId,
                        Title = a.Title,
                        Description = a.Description,
                        Deadline = a.Deadline,
                        fileURLs = a.File == null ? null : new AssignmentFileDto
                        {
                            URL = a.File.URL,
                            Title = a.File.Title
                        }
                    }).ToList(),
                    Instructors = course.CourseInstructors.Select(ci => ci.Instructor.Name).ToList(),
                    students = course.StudentCourses.Select(sc => sc.Student.Name).ToList(),
                    CourseCode = course.CourseCode,
                    CourseName = course.Name,
                    Description = course.Description
                };
            }
            catch (Exception ex)
            {
                throw new Exception( "An error occurred, Admin not vaild");
            }
        }

    }
}
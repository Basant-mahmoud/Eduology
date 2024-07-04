using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Eduology.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository

    {
        private readonly EduologyDBContext _context;
        public CourseRepository(EduologyDBContext context)
        {
            _context = context;
        }
        public async Task<Course> CreateAsync(Course course,string adminId)
        {
            var organization = await _context.Organizations
                                                 .Include(o => o.Courses)
                                                 .FirstOrDefaultAsync(o => o.OrganizationID == course.OrganizationID);
            var admin =await isAdminExistAsync(adminId);

            if (admin.Courses == null)
            {
                admin.Courses = new List<Course>();
            }

            if (organization.Courses == null)
            {
                organization.Courses = new List<Course>();
            }
            admin.Courses.Add(course);
            organization.Courses.Add(course);
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<bool> ExistsByCourseCodeAsync(string courseCode)
        {
            return await _context.Courses.AnyAsync(c => c.CourseCode == courseCode);
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return false;
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Course>> GetAllAsync(string userId,string role)
        {
           IQueryable<Course> coursesQuery = _context.Courses
                .Include(c => c.CourseInstructors)
                    .ThenInclude(ci => ci.Instructor)
                .Include(c => c.StudentCourses)
                    .ThenInclude(sc => sc.Student)
                .Include(c => c.Assignments); 


            if (role == "Instructor")
            {
                coursesQuery = coursesQuery.Where(c => c.CourseInstructors.Any(ci => ci.InstructorId == userId));
            }
            else if (role == "Student")
            {
                coursesQuery = coursesQuery.Where(c => c.StudentCourses.Any(sc => sc.StudentId == userId));
            }

            return await coursesQuery.ToListAsync();
        }
        

        public async Task<CourseDetailsDto> GetByIdAsync(String id)
        {
            var course = await _context.Courses
         .Include(c => c.CourseInstructors)
         .ThenInclude(ci => ci.Instructor)
         .Include(c => c.StudentCourses)
         .ThenInclude(sc => sc.Student)
         .Include(c => c.Assignments)  
         .ThenInclude(a => a.File)    
         .FirstOrDefaultAsync(c => c.id == id);
            if (course == null)
                return null;
            return new CourseDetailsDto
            {
                CourseId = course.id,
                CourseName = course.Name,
                CourseCode = course.CourseCode,
                Instructors = course.CourseInstructors.Select(ci => ci.Instructor.Name).ToList() ?? new List<string>(),
                students = course.StudentCourses.Select(sc => sc.Student.Name).ToList() ?? new List<string>(),
                assignments = course.Assignments.Select(a => new AssignmentDto
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
                }).ToList() ?? new List<AssignmentDto>()
            };
        }
        public async Task<Course> UpdateAsync(String id, CourseDto course)
        {
            var _course = await _context.Courses.FindAsync(id);
            if (_course == null)
                return null;
            _course.Name = course.Name;
            await _context.SaveChangesAsync();
            return _course;
        }
        async Task<CourseDetailsDto> ICourseRepository.GetByNameAsync(string name)
        {
            var course = await _context.Courses
       .Include(c => c.CourseInstructors)
          .ThenInclude(ci => ci.Instructor)
       .Include(c => c.StudentCourses)
          .ThenInclude(sc => sc.Student)
       .Include(c => c.Assignments)
          .ThenInclude(a => a.File)
       .FirstOrDefaultAsync(c => c.Name == name);
            if (course == null)
                return null;
            return new CourseDetailsDto
            {
                CourseId = course.id,
                CourseName = course.Name,
                CourseCode = course.CourseCode,
                Instructors = course.CourseInstructors.Select(ci => ci.Instructor.Name).ToList() ?? new List<string>(),
                students = course.StudentCourses.Select(sc => sc.Student.Name).ToList() ?? new List<string>(),
                assignments = course.Assignments.Select(a => new AssignmentDto
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
                }).ToList() ?? new List<AssignmentDto>()
            };
        }

        public async Task<bool> OrganizationExistsAsync(int organizationId)
        {
            return await _context.Organizations.AnyAsync(o => o.OrganizationID == organizationId);
        }
        
        public async Task<bool> IsInstructorAssignedToCourse(string instructorId, string courseId)
        {
            var courseInstructor = await _context.courseInstructors
                .FirstOrDefaultAsync(ci => ci.InstructorId == instructorId && ci.CourseId == courseId);

            return courseInstructor != null;
        }
        public async Task<bool> ISStudentAssignedToCourse(string StudentId, string courseId)
        {
            var courseInstructor = await _context.StudentCourses
                .FirstOrDefaultAsync(ci => ci.StudentId == StudentId && ci.CourseId == courseId);

            return courseInstructor != null;
        }
        public async Task<bool> IsInstructorAssignedToCourseByName(string instructorId, string courseName)
        {
            var courseInstructor = await _context.courseInstructors
                .Include(ci => ci.Instructor)
                .Include(ci => ci.course)
        .FirstOrDefaultAsync(ci => ci.InstructorId == instructorId && ci.course.Name == courseName);

            return courseInstructor != null;
        }

        public async Task<bool> IStudentAssignedToCourseByName(string studentId, string courseName)
        {
            var studentCourse = await _context.StudentCourses
                .Include(sc => sc.Student)
                .Include(sc => sc.Course)
                .FirstOrDefaultAsync(sc => sc.Student.Id == studentId && sc.Course.Name == courseName);

            return studentCourse != null;
        }
        public async Task<bool> IsUserAssignedToCourseAsync(string userId, string courseId, string role)
        {
            if (role == "Instructor")
            {
                return await IsInstructorAssignedToCourse(userId, courseId);
            }
            else if (role == "Student")
            {
                return await ISStudentAssignedToCourse(userId, courseId);
            }
            return false;
        }
        public async Task<bool> IsUserAssignedToCourseAsyncByNmae(string userId, string name, string role)
        {
            if (role == "Instructor")
            {
                return await IsInstructorAssignedToCourseByName(userId, name);
            }
            else if (role == "Student")
            {
                return await IStudentAssignedToCourseByName(userId, name);
            }
            return false;
        }

        public async Task<List<Course>> GetAllByOrganizationIdAsync(int organizationId)
        {
            var courseList = await _context.Courses
                                           .Where(c => c.OrganizationID == organizationId)
                                           .ToListAsync();
            return courseList;
        }
        public async Task<Course> GetByIdForAdminAsync(string courseId, string adminId)
        {
            var admin = await _context.Users
                                      .Include(a => a.Courses)
                                      .ThenInclude(c => c.CourseInstructors)
                                      .ThenInclude(ci => ci.Instructor)
                                      .Include(a => a.Courses)
                                      .ThenInclude(c => c.StudentCourses)
                                      .ThenInclude(sc => sc.Student)
                                      .FirstOrDefaultAsync(a => a.Id == adminId);

            if (admin == null)
            {
                throw new Exception("Admin does not exist");
            }

            var course = admin.Courses?.FirstOrDefault(c => c.id == courseId);
            if (course == null)
            {
                throw new Exception("Course not found");
            }

            return course;
        }
        public async  Task<ApplicationUser> isAdminExistAsync(string AdminId)
        {
            var admin = await _context.Users
                            .FirstOrDefaultAsync(u => u.Id == AdminId);

            if (admin == null)
            {
                return null;
            }
            return admin;
        }
        public async Task<List<Course>> GetCoursesByStudentIdAsync(string studentId)
        {
            return await _context.Courses
                .Include(c => c.StudentCourses)
                .Where(c => c.StudentCourses.Any(sc => sc.StudentId == studentId))
                .ToListAsync();
        }

    }

}

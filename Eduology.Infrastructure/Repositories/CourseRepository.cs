using System;
using System.Collections.Generic;
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
        private readonly UserManager<ApplicationUser> _userManager;
        public CourseRepository(EduologyDBContext context)
        {
            _context = context;
        }
        public async Task<Course> CreateAsync(CourseDto courseDto)
        {
            var course = new Course
            {
                CourseCode = courseDto.CourseCode,
                Name = courseDto.Name,
                Year = courseDto.Year,
            };

            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return course;

        }


        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return false;
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CourseDetailsDto>> GetAllAsync()
        {
            var courses = await _context.Courses
                .Include(c => c.CourseInstructors)
                    .ThenInclude(ci => ci.Instructor)
                .Include(c => c.StudentCourses)
                    .ThenInclude(sc => sc.Student)
                .ToListAsync();

            return courses.Select(c => new CourseDetailsDto
            {
                CourseId = c.CourseId,
                Name = c.Name,
                CourseCode = c.CourseCode,
                Instructors = c.CourseInstructors.Select(ci => ci.Instructor.Name).ToList(),
                students = c.StudentCourses.Select(sc => sc.Student.Name).ToList()
            }).ToList();

        }

        public async Task<CourseDetailsDto> GetByIdAsync(int id)
        {
            var course = await _context.Courses
                .Include(c => c.CourseInstructors)
                    .ThenInclude(ci => ci.Instructor)
                .Include(c => c.StudentCourses)
                    .ThenInclude(sc => sc.Student)
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
                return null;

            return new CourseDetailsDto
            {
                CourseId = course.CourseId,
                Name = course.Name,
                CourseCode = course.CourseCode,
                Instructors = course.CourseInstructors.Select(ci => ci.Instructor.Name).ToList(),
                students = course.StudentCourses.Select(sc => sc.Student.Name).ToList()
            };

        }
        public async Task<bool> UpdateAsync(int id, CourseDto course)
        {
            var _course = await _context.Courses.FindAsync(id);
            if (_course == null)
                return false;
            _course.Name = course.Name;
            _course.CourseCode = course.CourseCode;
            _course.Year = course.Year;
            _context.SaveChanges();
            return true;
        }

        async Task<CourseDetailsDto> ICourseRepository.GetByNameAsync(string name)
        {
            var course = await _context.Courses
                .Include(c => c.CourseInstructors)
                    .ThenInclude(ci => ci.Instructor)
                .Include(c => c.StudentCourses)
                    .ThenInclude(sc => sc.Student)
                .FirstOrDefaultAsync(c => c.Name == name);

            if (course == null)
                return null;

            return new CourseDetailsDto
            {
                CourseId = course.CourseId,
                Name = course.Name,
                CourseCode = course.CourseCode,
                Instructors = course.CourseInstructors.Select(ci => ci.Instructor.Name).ToList(),
                students = course.StudentCourses.Select(sc => sc.Student.Name).ToList()
            };
        }
    }
}
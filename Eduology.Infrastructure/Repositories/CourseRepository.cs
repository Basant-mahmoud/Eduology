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
        public async Task Createsync(Course course)
        {
            _context.Courses.AddAsync(course);
            _context.SaveChanges();

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course  = await _context.Courses.FindAsync(id);
            if (course == null)
                return false;
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CourseDto>> GetAllAsync()
        {
            var courses = await _context.Courses.ToListAsync();
            if (courses == null)
                return new List<CourseDto>();
            var result = new List<CourseDto>();
            foreach(var Course in courses)
            {
                result.Add(
                new CourseDto
                {
                    CourseId = Course.CourseId,
                    Name = Course.Name,
                });
            }
            return result;

        }

        public async Task<CourseDto> GetByIdAsync(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null)
                return null;
            return new CourseDto 
            { 
                CourseId = course.CourseId,
                Name = course.Name,
                InstructorId = course.InstructorId,
                studentCourses = course.StudentCourses,
            };

        }
        public async Task<bool> UpdateAsync(CourseDto course)
        {
            var _course = await _context.Courses.FindAsync(course.CourseId);
            if (_course == null)
                return false;
            _course.CourseId = course.CourseId;
            _course.Name = course.Name;
            _course.InstructorId = course.InstructorId;
            _course.StudentCourses = course.studentCourses;

            return true;
        }

        Task<CourseDto> ICourseRepository.GetByNameAsync(string name)
        {
            var course = _context.Courses
                      .Include(c => c.StudentCourses)
                      .FirstOrDefaultAsync(c => c.Name == name);

        if (course == null)
        {
            return null;
        }

            return new CourseDto
            {
                Name = course.Name,
                InstructorId = course.InstructorId,
                CourseId = course.CourseId,
                studentCourses = course.StudentCourses,
            };
        }
    }
}

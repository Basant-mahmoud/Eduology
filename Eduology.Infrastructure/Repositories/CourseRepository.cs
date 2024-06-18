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
            var course  = await _context.Courses.FindAsync(id);
            if (course == null)
                return false;
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CourseDetailsDto>> GetAllAsync()
        {
            var courses = await _context.Courses.ToListAsync();
            if (courses == null)
                return null;
            var result = new List<CourseDetailsDto>();
            foreach(var Course in courses)
            {
                result.Add(
                new CourseDetailsDto
                {
                    CourseId = Course.CourseId,
                    Name = Course.Name,
                    CourseCode = Course.CourseCode,
                    studentCourses = Course.StudentCourses,
                });
            }
            return result;

        }

        public async Task<CourseDetailsDto> GetByIdAsync(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null)
                return null;
            return new CourseDetailsDto 
            { 
                CourseId = course.CourseId,
                Name = course.Name,
                CourseCode= course.CourseCode,
                studentCourses = course.StudentCourses,
            };

        }
        public async Task<bool> UpdateAsync(int id,CourseDto course)
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
            var _course = await _context.Courses.FirstOrDefaultAsync(c => c.Name == name);

            if (_course == null)
            {
               return null;
            }

            return new CourseDetailsDto
            {
                Name = _course.Name,
                CourseCode = _course.CourseCode,
                CourseId = _course.CourseId,
                studentCourses = _course.StudentCourses,
            };
        }
    }
}

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
        public CourseRepository(EduologyDBContext context)
        {
            _context = context;
        }
        public async Task<Course> CreateAsync(Course course)
        {
            // Check if the organization exists
            if (!await OrganizationExistsAsync(course.OrganizationID))
            {
                throw new KeyNotFoundException("Organization not found."); // Handle this case according to your application's error handling strategy
                //return false;
            }

            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<bool> ExistsByCourseCodeAsync(string courseCode)
        {
            return await _context.Courses.AnyAsync(c => c.CourseCode == courseCode);
        }
        public async Task<Course> DeleteAsync(string id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return null;
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<IEnumerable<CourseDetailsDto>> GetAllAsync()
        {
            var courses = await _context.Courses
                .Include(c => c.CourseInstructors)
                    .ThenInclude(ci => ci.Instructor)
                .Include(c => c.StudentCourses)
                    .ThenInclude(sc => sc.Student)
                .ToListAsync();
            if (courses == null || courses.Count == 0)
                return null;
            return courses.Select(c => new CourseDetailsDto
            {
                CourseId = c.CourseId,
                Name = c.Name,
                CourseCode = c.CourseCode,
                Instructors = c.CourseInstructors.Select(ci => ci.Instructor.Name).ToList(),
                students = c.StudentCourses.Select(sc => sc.Student.Name).ToList()
            }).ToList();

        }

        public async Task<CourseDetailsDto> GetByIdAsync(String id)
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
        public async Task<Course> UpdateAsync(String id, CourseDto course)
        {
            var _course = await _context.Courses.FindAsync(id);
            if (_course == null)
                return null;
            _course.Name = course.Name;
            _course.CourseCode = course.CourseCode;
            _course.Year = course.Year;
            _context.SaveChanges();
            return _course;
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

        public async Task<bool> OrganizationExistsAsync(int organizationId)
        {
            return await _context.Organizations.AnyAsync(o => o.OrganizationID == organizationId);
        }

        public async Task<bool> AddMateriaCourseAsync( Material material)
        {
            var course = await _context.Courses.FindAsync(material.CourseId);
            if (course == null)
                return false;

            // Check if the material type exists and assign it
            var materialType = await _context.MaterialTypes.FirstOrDefaultAsync(t => t.Name == material.MaterialType.Name);
            if (materialType == null)
            { 
                return false;
            }
            course.Materials ??= new List<Material>();
            course.Materials.Add(material);

            await _context.SaveChangesAsync();

            return true;
        }   
    }
}
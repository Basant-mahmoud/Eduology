using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EduologyDBContext _context;

        public StudentRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, EduologyDBContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<UserDto> GetStudentByIdAsync(string studentId)
        {
            var student = await _context.Users.FirstOrDefaultAsync(u => u.Id == studentId);
            return await MapUserToDtoAsync(student);
        }
        public async Task<IEnumerable<UserDto>> GetAllStudentsAsync()
        {
            var studentRole = await _roleManager.FindByNameAsync("Student");
            if (studentRole == null)
                return new List<UserDto>();

            var students = await _userManager.GetUsersInRoleAsync("Student");
            var result = new List<UserDto>();

            foreach (var student in students)
            {
                var dto = await MapUserToDtoAsync(student);
                result.Add(dto);
            }

            return result;
        }
        public async Task<bool> UpdateStudentAsync(string studentId, UserDto userDto)
        {
            var student = await _userManager.FindByIdAsync(studentId);
            if (student == null)
            {
                return false;
            }
            student.Name = userDto.Name;
            student.UserName = userDto.UserName;
            student.Email = userDto.Email;

            var result = await _userManager.UpdateAsync(student);
            return result.Succeeded;
        }
        public async Task<bool> DeleteStudentAsync(string studentId)
        {
            var student = await _context.Users.FindAsync(studentId);
            if(student == null)
            { 
                return false; 
            }
            _context.Users.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RegisterToCourseAsync(string studentId, string courseCode)
        {
            var student = await _context.Users.FindAsync(studentId);
            if (student == null)
            {
                throw new Exception("Student not found in database");
            }
            var course = await _context.Courses
                .Include(c => c.StudentCourses)
                .FirstOrDefaultAsync(c => c.CourseCode == courseCode);
            if (course == null)
            {
                throw new Exception("course not found ");
            }
            // Check if the student is already assigned to the course
            if (course.StudentCourses.Any(ci => ci.StudentId == studentId))
            {
                throw new Exception("Student already register in course");
            }
            var courseStudent = new StudentCourse
            {
                StudentId = studentId,
                CourseId = course.id
            };

            _context.StudentCourses.Add(courseStudent);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<List<Course>> GetAllCourseToSpecificStudentAsync(string studentId)
        {
            var student = await _context.Users.FindAsync(studentId);
            if (student == null)
            {
                return null;
            }

            var courseStudents = await _context.StudentCourses
                .Where(sc => sc.StudentId == studentId)
                .Include(sc => sc.Course.CourseInstructors)
                    .ThenInclude(ci => ci.Instructor)
                .Include(sc => sc.Course.StudentCourses)
                    .ThenInclude(sc => sc.Student)
                .Select(sc => sc.Course)
                .ToListAsync();

            if (courseStudents == null || !courseStudents.Any())
            {
                return new List<Course>();
            }

            return courseStudents;
        }

        private async Task<UserDto> MapUserToDtoAsync(ApplicationUser user)
        {
            if (user == null)
                return null;

            await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email
            };
        }
    }
}
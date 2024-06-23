using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Repositories
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EduologyDBContext _context;

        public InstructorRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, EduologyDBContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetAllInstructorsAsync()
        {
            var instructorRole = await _roleManager.FindByNameAsync("Instructor");
            if (instructorRole == null)
                return new List<UserDto>();

            var instructors = await _userManager.GetUsersInRoleAsync("Instructor");
            var result = new List<UserDto>();

            foreach (var instructor in instructors)
            {
                var dto = await MapUserToDtoAsync(instructor);
                result.Add(dto);
            }
            return result;
        }
       

        public async Task<UserDto> GetInstructorByIdAsync(string id)
        {
            var instructor = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return await MapUserToDtoAsync(instructor);
        }

        public async Task<UserDto> GetInstructorByNameAsync(string name)
        {
            var instructor = await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
            return await MapUserToDtoAsync(instructor);
        }

        public async Task<UserDto> GetInstructorByUserNameAsync(string userName)
        {
            var instructor = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            return await MapUserToDtoAsync(instructor);
        }

        public async Task<bool> DeleteInstructorAsync(string id)
        {
            var instructor = await _context.Users.FindAsync(id);
            if (instructor == null)
            {
                throw new KeyNotFoundException("Instructor not found.");
            }

            _context.Users.Remove(instructor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateInstructorAsync(string id, UserDto updateInstructorDto)
        {
            var instructor = await _context.Users.FindAsync(id);
            if (instructor == null)
            {
                //throw new KeyNotFoundException("Instructor not found.");
                return false;
            }

            if (!string.IsNullOrEmpty(updateInstructorDto.Name))
            {
                instructor.Name = updateInstructorDto.Name;
            }

            if (!string.IsNullOrEmpty(updateInstructorDto.UserName))
            {
                instructor.UserName = updateInstructorDto.UserName;
            }

            if (!string.IsNullOrEmpty(updateInstructorDto.Email))
            {
                instructor.Email = updateInstructorDto.Email;
            }

            _context.Users.Update(instructor);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<UserDto> MapUserToDtoAsync(ApplicationUser user)
        {
            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email
            };
        }
        public async Task<bool> RegisterToCourseAsync(string instructorId, string courseCode)
        {
            var instructor = await _context.Users.FindAsync(instructorId);
            if (instructor == null)
                return false;
            var course = await _context.Courses
                .Include(c => c.CourseInstructors)
                .FirstOrDefaultAsync(c => c.CourseCode == courseCode);
            if (course == null)
                return false;
            // Check if the instructor is already assigned to the course
            if (course.CourseInstructors.Any(ci => ci.InstructorId == instructorId))
                return true;

            var courseInstructor = new CourseInstructor
            {
                InstructorId = instructorId,
                CourseId = course.CourseId
            };

            _context.courseInstructors.Add(courseInstructor);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<CourseUserDto>> GetAllCourseToSpecificInstructorAsync(string InstructorId)
        {
            var instructor = await _context.Users.FindAsync(InstructorId);
            if (instructor == null)
            {
                return null; 
            }

            var courseInstructors = await _context.courseInstructors
                .Where(ci => ci.InstructorId == InstructorId)
                .Include(ci => ci.course) 
                .Select(ci => ci.course)
                .ToListAsync();

            var courseDtos = courseInstructors.Select(course => new CourseUserDto
            {
                Name = instructor.Name,
                CourseName = course.Name,
                CourseDescription = course.Description,
                year = course.Year 
            }).ToList();

            return courseDtos;
        }

       
    }
}

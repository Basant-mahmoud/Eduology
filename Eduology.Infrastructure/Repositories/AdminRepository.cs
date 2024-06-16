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
    public class AdminRepository : IAdminRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EduologyDBContext _context; 

        public AdminRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, EduologyDBContext context)
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
                // Load instructor's courses
                await _context.Entry(instructor)
                    .Collection(u => u.Courses)
                    .LoadAsync();

                var numberOfCourses = instructor.Courses.Count;

                var dto = new UserDto
                {
                    Id = instructor.Id,
                    Name = instructor.Name,
                    UserName = instructor.UserName,
                    Email = instructor.Email,
                    NumberOfCourses = numberOfCourses
                };

                result.Add(dto);
            }

            return result;
        }
        public async Task<UserDto> GetInstructorByIdAsync(string id)
        {
            var instructor = await _context.Users
                .Include(u => u.Courses) 
                .FirstOrDefaultAsync(u => u.Id == id);

            if (instructor == null)
                return null;

            await _userManager.GetRolesAsync(instructor);

            var numberOfCourses = instructor.Courses?.Count ?? 0;

            return new UserDto
            {
                Id = instructor.Id,
                Name = instructor.Name,
                UserName = instructor.UserName,
                Email = instructor.Email,
                NumberOfCourses = numberOfCourses
            };
        }
        public async Task<UserDto> GetInstructorByNameAsync(string Name)
        {
            var instructor = await _context.Users
                .Include(u => u.Courses)
                .FirstOrDefaultAsync(u => u.Name == Name);

            if (instructor == null)
                return null;

            await _userManager.GetRolesAsync(instructor);

            var numberOfCourses = instructor.Courses?.Count ?? 0;

            return new UserDto
            {
                Id = instructor.Id,
                Name = instructor.Name,
                UserName = instructor.UserName,
                Email = instructor.Email,
                NumberOfCourses = numberOfCourses
            };
        }
        public async Task<UserDto> GetInstructorByUserNameAsync(string UserName)
        {
            var instructor = await _context.Users
                .Include(u => u.Courses)
                .FirstOrDefaultAsync(u => u.UserName == UserName);

            if (instructor == null)
                return null;

            await _userManager.GetRolesAsync(instructor);

            var numberOfCourses = instructor.Courses?.Count ?? 0;

            return new UserDto
            {
                Id = instructor.Id,
                Name = instructor.Name,
                UserName = instructor.UserName,
                Email = instructor.Email,
                NumberOfCourses = numberOfCourses
            };
        }
        public async Task<bool> DeleteInstructorAsync(string id)
        {
            var instructor = await _context.Users.FindAsync(id);
            if (instructor == null)
            {
                return false;
            }

            _context.Users.Remove(instructor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
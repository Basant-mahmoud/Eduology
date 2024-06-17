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
            var instructor = await _context.Users
                .Include(u => u.Courses)
                .FirstOrDefaultAsync(u => u.Id == id);

            return await MapUserToDtoAsync(instructor);
        }
        public async Task<UserDto> GetInstructorByNameAsync(string name)
        {
            var instructor = await _context.Users
                .Include(u => u.Courses)
                .FirstOrDefaultAsync(u => u.Name == name);

            return await MapUserToDtoAsync(instructor);
        }
        public async Task<UserDto> GetInstructorByUserNameAsync(string userName)
        {
            var instructor = await _context.Users
                .Include(u => u.Courses)
                .FirstOrDefaultAsync(u => u.UserName == userName);

            return await MapUserToDtoAsync(instructor);
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
        public async Task<bool> UpdateInstructorAsync(string id, UpdateUserDto updateInstructorDto)
        {
            var instructor = await _context.Users.FindAsync(id);
            if (instructor == null)
            {
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

            await _userManager.GetRolesAsync(user);
            var numberOfCourses = user.Courses?.Count ?? 0;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email,
                NumberOfCourses = numberOfCourses
            };
        }

       
    }
}
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
            var student = await _context.Users
                .Include(u => u.Courses)
                .FirstOrDefaultAsync(u => u.Id == studentId);

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
        public async Task<bool> DeleteAsync(string studentId)
        {
            var student = await _context.Users.FindAsync(studentId);
            if (student == null)
            {
                return false;
            }

            _context.Users.Remove(student);
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
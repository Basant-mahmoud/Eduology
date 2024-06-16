using Eduology.Application.Services;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _AdminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _AdminRepository = adminRepository;
        }

        [HttpGet("GetAllInstructors")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetInstructors()
        {
            var instructors = await _AdminRepository.GetAllInstructorsAsync();
            if (instructors == null || !instructors.Any())
            {
                return NotFound();
            }

            return Ok(instructors);
        }
        [HttpGet("SearchInstructorbyId/{id}")]
        public async Task<ActionResult<UserDto>> GetInstructorById(string id)
        {
            var instructor = await _AdminRepository.GetInstructorByIdAsync(id);
            if (instructor == null)
            {
                return NotFound(); // Or appropriate HTTP status code
            }

            return Ok(instructor);
        }
        [HttpGet("SearchInstructorbyName/{name}")]
        public async Task<ActionResult<UserDto>> GetInstructorByName(string name)
        {
            var instructor = await _AdminRepository.GetInstructorByNameAsync(name);
            if (instructor == null)
            {
                return NotFound();
            }

            return Ok(instructor);
        }
        [HttpGet("SearchInstructorbyUserName/{username}")]
        public async Task<ActionResult<UserDto>> GetInstructorByUserName(string username)
        {
            var instructor = await _AdminRepository.GetInstructorByUserNameAsync(username);
            if (instructor == null)
            {
                return NotFound();
            }

            return Ok(instructor);
        }
        [HttpDelete("deleteInstructor/{id}")]
        public async Task<IActionResult> DeleteInstructor(string id)
        {
            var result = await _AdminRepository.DeleteInstructorAsync(id);
            if (!result)
            {
                return NotFound(); // Or appropriate HTTP status code
            }

            return Ok(new { message = "Instructor deleted successfully" });
        }
    }
}

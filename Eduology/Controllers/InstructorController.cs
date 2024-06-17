using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorRepository _InstructorRepository;

        public InstructorController(IInstructorRepository InstructorRepository)
        {
            _InstructorRepository = InstructorRepository;
        }

        [HttpGet("GetAllInstructors")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetInstructors()
        {
            var instructors = await _InstructorRepository.GetAllInstructorsAsync();
            if (instructors == null || !instructors.Any())
            {
                return NotFound();
            }

            return Ok(instructors);
        }
        [HttpGet("SearchInstructorbyId/{id}")]
        public async Task<ActionResult<UserDto>> GetInstructorById(string id)
        {
            var instructor = await _InstructorRepository.GetInstructorByIdAsync(id);
            if (instructor == null)
            {
                return NotFound(); 
            }

            return Ok(instructor);
        }
        [HttpGet("SearchInstructorbyName/{name}")]
        public async Task<ActionResult<UserDto>> GetInstructorByName(string name)
        {
            var instructor = await _InstructorRepository.GetInstructorByNameAsync(name);
            if (instructor == null)
            {
                return NotFound();
            }

            return Ok(instructor);
        }
        [HttpGet("SearchInstructorbyUserName/{username}")]
        public async Task<ActionResult<UserDto>> GetInstructorByUserName(string username)
        {
            var instructor = await _InstructorRepository.GetInstructorByUserNameAsync(username);
            if (instructor == null)
            {
                return NotFound();
            }

            return Ok(instructor);
        }
        [HttpPut("UpdateInstructor/{id}")]
        public async Task<IActionResult> UpdateInstructor(string id, [FromBody] UserDto updateInstructorDto)
        {
            var result = await _InstructorRepository.UpdateInstructorAsync(id, updateInstructorDto);
            if (!result)
            {
                return NotFound();
            }

            var updatedInstructor = await _InstructorRepository.GetInstructorByIdAsync(id);
            return Ok(updatedInstructor);
        }
        [HttpDelete("deleteInstructor/{id}")]
        public async Task<IActionResult> DeleteInstructor(string id)
        {
            var result = await _InstructorRepository.DeleteInstructorAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok(new { message = "Instructor deleted successfully" });
        }
    }
}

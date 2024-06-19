using Eduology.Application.Interface;
using Eduology.Application.Services.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _instructorService;

        public InstructorController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [HttpGet("GetAllInstructors")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetInstructors()
        {
            var instructors = await _instructorService.GetAllInstructorsAsync();
            if (instructors == null || !instructors.Any())
            {
                return Ok(new List<UserDto>());
            }
            else
            {
                return Ok(instructors); // Return list of instructors as Ok result
            }
        }

        [HttpGet("GetInstructorById/{id}")]
        public async Task<ActionResult<UserDto>> GetInstructorById(string id)
        {
            var instructor = await _instructorService.GetInstructorByIdAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }

            return Ok(instructor);
        }

        [HttpGet("SearchInstructorByName/{name}")]
        public async Task<ActionResult<UserDto>> GetInstructorByName(string name)
        {
            var instructor = await _instructorService.GetInstructorByNameAsync(name);
            if (instructor == null)
            {
                return NotFound();
            }

            return Ok(instructor);
        }

        [HttpGet("SearchInstructorByUserName/{username}")]
        public async Task<ActionResult<UserDto>> GetInstructorByUserName(string username)
        {
            var instructor = await _instructorService.GetInstructorByUserNameAsync(username);
            if (instructor == null)
            {
                return NotFound();
            }

            return Ok(instructor);
        }

        [HttpPut("UpdateInstructor/{id}")]
        public async Task<IActionResult> UpdateInstructor(string id, [FromBody] UserDto updateInstructorDto)
        {
            var result = await _instructorService.UpdateInstructorAsync(id, updateInstructorDto);
            if (!result)
            {
                return NotFound();
            }

            var updatedInstructor = await _instructorService.GetInstructorByIdAsync(id);
            return Ok(updatedInstructor);
        }

        [HttpDelete("DeleteInstructor/{id}")]
        public async Task<IActionResult> DeleteInstructor(string id)
        {
            var result = await _instructorService.DeleteInstructorAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok(new { message = "Instructor deleted successfully" });
        }
        [HttpPost("RegisterToCourse")]
        public async Task<IActionResult> RegisterToCourse([FromBody] RegisterToCourseDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _instructorService.RegisterToCourseAsync(model.InstructorId, model.CourseCode);
            if (success)
                return Ok("Instructor added to the course successfully.");
            else
                return BadRequest("Failed to add instructor to the course.");
        }
    }
}

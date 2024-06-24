using Eduology.Application.Interface;
using Eduology.Application.Services.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetInstructors()
        {
            var instructors = await _instructorService.GetAllInstructorsAsync();
            if (instructors == null || !instructors.Any())
            {
                return Ok(new List<UserDto>());
            }
            else
            {
                return Ok(instructors); 
            }
        }
       

        [HttpGet("GetInstructorById/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> GetInstructorById(string id)
        {
            var instructor = await _instructorService.GetInstructorByIdAsync(id);
            if (instructor == null)
            {
                return NotFound(new { message = $"Instructor id {id} not found" });
            }

            return Ok(instructor);
        }

        [HttpGet("SearchInstructorByName/{name}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> GetInstructorByName(string name)
        {
            var instructor = await _instructorService.GetInstructorByNameAsync(name);
            if (instructor == null)
            {
                return NotFound(new { message = $"Instructor name {name} not found" });
            }

            return Ok(instructor);
        }

        [HttpGet("SearchInstructorByUserName/{username}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> GetInstructorByUserName(string username)
        {
            var instructor = await _instructorService.GetInstructorByUserNameAsync(username);
            if (instructor == null)
            {
                return NotFound(new { message = $"Instructor user name {username} not found" });
            }

            return Ok(instructor);
        }

        [HttpPut("UpdateInstructor/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateInstructor(string id, [FromBody] UserDto updateInstructorDto)
        {
            var result = await _instructorService.UpdateInstructorAsync(id, updateInstructorDto);
            if (!result)
            {
                return NotFound(new { message = $"Instructor Id {id} not found" });
            }

            var updatedInstructor = await _instructorService.GetInstructorByIdAsync(id);
            return Ok(updatedInstructor);
        }

        [HttpDelete("DeleteInstructor/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteInstructor(string id)
        {
            var result = await _instructorService.DeleteInstructorAsync(id);
            if (!result)
            {
                return NotFound(new { message = $"Instructor Id {id} not found" });
            }

            return Ok(new { message = "Instructor deleted successfully" });
        }
        /// 
        [HttpPost("RegisterToCourse")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> RegisterToCourse([FromBody] RegisterInstructorToCourseDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _instructorService.RegisterToCourseAsync(userId, model.CourseCode);
            if (success)
                return Ok(new { message = "Instructor added to the course successfully." });
            else
                return BadRequest(new { message = "Failed to add instructor to the course." });
        }
        [HttpGet("AllCoursetoInstructor")]
        [Authorize(Roles = "Instructor")]
        public async Task<ActionResult<CourseUserDto>> AllCoursetoInstructor()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var instructor = await _instructorService.GetAllCourseToSpecificInstructorAsync(userId);
            if (instructor == null)
            {
                return NotFound(new { message = $"Instructor Id {userId} not found" });
            }

            return Ok(instructor);
        }
       
    }
}

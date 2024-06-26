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
        private string GetUserId()
        {
            return User.FindFirst("uid")?.Value;
        }

        [HttpGet("GetAllInstructors")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetInstructors()
        {
            try
            {
                var instructors = await _instructorService.GetAllInstructorsAsync();
                return Ok(instructors);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
       

        [HttpGet("GetInstructorById/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> GetInstructorById(string id)
        {
            try
            {
                var instructor = await _instructorService.GetInstructorByIdAsync(id);
                return Ok(instructor);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
           

           
        }

        [HttpGet("SearchInstructorByName/{name}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> GetInstructorByName(string name)
        {
            try
            {
                var instructor = await _instructorService.GetInstructorByNameAsync(name);

                return Ok(instructor);

            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });

            }

        }

        [HttpGet("SearchInstructorByUserName/{username}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> GetInstructorByUserName(string username)
        {
            try
            {
                var instructor = await _instructorService.GetInstructorByUserNameAsync(username);
                return Ok(instructor);


            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });

            }

        }

        [HttpPut("UpdateInstructor/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateInstructor(string id, [FromBody] UserDto updateInstructorDto)
        {
            try
            {
                var result = await _instructorService.UpdateInstructorAsync(id, updateInstructorDto);
                var updatedInstructor = await _instructorService.GetInstructorByIdAsync(id);
                return Ok(updatedInstructor);

            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });

            }
        }

        [HttpDelete("DeleteInstructor/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteInstructor(string id)
        {
            try
            {
                var result = await _instructorService.DeleteInstructorAsync(id);
                return Ok(new { message = "Instructor deleted successfully" });

            }

            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });

            }

        }
        /// 
        [HttpPost("RegisterToCourse")]
       [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> RegisterToCourse([FromBody] RegisterUserToCourseDto model)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var success = await _instructorService.RegisterToCourseAsync(userId, model.CourseCode);
                    return Ok(new { message = "Instructor added to the course successfully." });

   
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });

            }
           
        }
        [HttpGet("AllCoursetoInstructor")]
        [Authorize(Roles = "Instructor")]
        public async Task<ActionResult<CourseUserDto>> AllCoursetoInstructor()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var instructor = await _instructorService.GetAllCourseToSpecificInstructorAsync(userId);
                return Ok(instructor);


            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });

            }

        }
       
       
    }
}

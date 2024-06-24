using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Repositories;
using Eduology.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CourseDto course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCourse = await _courseService.CreateAsync(course);
            if (createdCourse == null)
                return BadRequest(ModelState);
            return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse }, createdCourse);
        }
        [Authorize(Roles = "Instructor,Student")]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetCourseById(String id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("User ID not found in the token");
            }
            var course = await _courseService.GetByIdAsync(id,userId);

            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }
        [Authorize(Roles = "Instructor,Student")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("User ID not found in the token");
            }
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role == null)
            {
                return Unauthorized("Role not found in the token");
            }
            var courses = await _courseService.GetAllAsync(userId,role);
            if (courses == null || !courses.Any())
            {
                return NoContent();
            }
            return Ok(courses);
        }
        [Authorize(Roles = "Instructor,Student")]
        [HttpGet("GetByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("User ID not found in the token");
            }
            CourseDetailsDto course = await _courseService.GetByNameAsync(name,userId);
            if (course == null)
                return NotFound();
            return Ok(course);
        }
        [Authorize(Roles = "Instructor")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(String id,[FromBody] CourseDto courseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _courseService.UpdateAsync(id,courseDto);
            if (updated == null)
            {
                return NotFound();
            }

            return Ok(new { message = "Course updated successfully" });
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
           var course =  await _courseService.DeleteAsync(id);
           if(!course)
                return NotFound(new { message = $"Course with id {id} not found." });
            return Ok(new { message = "Course deleted successfully" });
        }
    }
}

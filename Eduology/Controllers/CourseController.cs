using Eduology.Application.Interface;
using Eduology.Application.Services.Helper;
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
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized("User ID not found in the token");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdCourse = await _courseService.CreateAsync(course, userId);
                if (createdCourse == null)
                    return BadRequest(ModelState);
                return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse }, createdCourse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
        [Authorize(Roles = "Instructor,Student")]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetCourseById(String id)
        {
            var userId = User.GetUserId();
            var role = User.GetUserRole();

            if (userId == null)
            {
                return Unauthorized("User ID not found in the token");
            }
            try
            {
                var course = await _courseService.GetByIdAsync(id, userId, role);

                if (course == null)
                {
                    return NotFound();
                }
                return Ok(course);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Instructor,Student")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized("User ID not found in the token");
            }
            var role = User.GetUserRole();
            if (role == null)
            {
                return Unauthorized("Role not found in the token");
            }
            try
            {
                var courses = await _courseService.GetAllAsync(userId, role);
                if (courses == null || !courses.Any())
                {
                    return NoContent();
                }
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Instructor,Student")]
        [HttpGet("GetByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var userId = User.GetUserId();
            var role = User.GetUserRole();

            if (userId == null)
            {
                return Unauthorized("User ID not found in the token");
            }
            try
            {
                CourseDetailsDto course = await _courseService.GetByNameAsync(name, userId, role);
                if (course == null)
                    return NotFound();
                return Ok(course);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(String id,[FromBody] CourseDto courseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updated = await _courseService.UpdateAsync(id, courseDto);
                if (!updated)
                {
                    return NotFound();
                }

                return Ok(new { message = "Course updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var course = await _courseService.DeleteAsync(id);
                if (!course)
                    return NotFound(new { message = $"Course with id {id} not found." });
                return Ok(new { message = "Course deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetByOrganizationId/{id}")]
        public async Task<IActionResult> GetByOrganizationId(int id)
        {
            try
            {
                var courses = await _courseService.GetAllByOrganizationIdAsync(id);
                if (courses == null)
                    return NotFound(new { message = $"Organization with id {id} not found." });
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetByIdForAdmin/{id}")]
        public async Task<IActionResult> GetByIdForAdmin(string id)
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized("Admin ID not found in the token");
            try
            {
                CourseDetailsDto course = await _courseService.GetByIdForAdminAsync(id, userId);
                return Ok(course);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Repositories;
using Eduology.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Create([FromBody] CourseCreationDto course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCourse = await _courseService.CreateAsync(course);
            if (createdCourse == null)
                return BadRequest(ModelState);
            return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.CourseId }, createdCourse);
        }
        [Authorize(Roles = "Instructor,Student")]
        [HttpGet("GetById/{id}/{UserId}")]
        public async Task<IActionResult> GetCourseById(String id,string UserId)
        {
            var course = await _courseService.GetByIdAsync(id,UserId);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }
        [Authorize(Roles = "Instructor,Student")]
        [HttpGet("GetAll/{UserID}/{CourseId}")]
        public async Task<IActionResult> GetAll(string UserID,string CourseId)
        {
            var courses = await _courseService.GetAllAsync(UserID,CourseId);
            if (courses == null || !courses.Any())
            {
                return NoContent();
            }
            return Ok(courses);
        }
        [Authorize(Roles = "Instructor,Student")]
        [HttpGet("GetByName/{name}/{UserID}/{CourseId}")]
        public async Task<IActionResult> GetByName(string name,string UserID,string CourseId)
        {
            CourseDetailsDto course = await _courseService.GetByNameAsync(name,UserID,CourseId);
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
           if(course == null)
                return Ok(new { message = "This course is not exist" });
           return Ok(new { message = "Course deleted successfully" });
        }
    }
}

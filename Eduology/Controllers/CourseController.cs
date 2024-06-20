using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Repositories;
using Eduology.Infrastructure.Services;
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

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CourseCreationDto course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCourse = await _courseService.CreateAsync(course);
            return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.CourseId }, createdCourse);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetCourseById(String id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.GetAllAsync();
            if (courses == null || !courses.Any())
            {
                return NoContent();
            }
            return Ok(courses);
        }
        [HttpGet("GetByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            CourseDetailsDto course = await _courseService.GetByNameAsync(name);
            if (course == null)
                return NotFound();
            return Ok(course);
        }
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

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
           var course =  await _courseService.DeleteAsync(id);
           if(course == null)
                return Ok(new { message = "This course is not exist" });
           return Ok(new { message = "Course deleted successfully" });
        }

        [HttpPost("AddMatrial")]
        public async Task<IActionResult> AddMatrial([FromBody] MaterialDto matrial)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _courseService.AddMaterialAsync(matrial);
            if (!success)
            {
                return BadRequest(new { message = "Failed to add material." });
            }

            return Ok(new { message = "Material added successfully." });
        }


    }
}

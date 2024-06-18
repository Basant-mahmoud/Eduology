using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        public CourseController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CourseDto course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Course _course = await _courseRepository.CreateAsync(course);
            course.CourseId = _course.CourseId;
            return CreatedAtAction(nameof(GetCourseById), new { id = _course.CourseId }, course);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseRepository.GetAllAsync();
            if (courses == null || !courses.Any())
            {
                return NotFound();
            }
            return Ok(courses);
        }
        [HttpGet("GetByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            CourseDetailsDto course = await _courseRepository.GetByNameAsync(name);
            if (course == null)
                return NotFound();
            return Ok(course);
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody] CourseDto courseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _courseRepository.UpdateAsync(id,courseDto);
            if (!updated)
            {
                return NotFound();
            }

            return Ok(new { message = "Course updated successfully" });
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
           var course =  await _courseRepository.DeleteAsync(id);
           if(!course)
                return NotFound();
           return Ok(new { message = "Course deleted successfully" });
        }

    }
}

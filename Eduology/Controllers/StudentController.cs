using Eduology.Application.Services.Helper;
using Eduology.Application.Services.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Infrastructure.Repositories;
using Eduology.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _StudentService;
        public StudentController(IStudentService studentService)
        {
            _StudentService = studentService;
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetStudents()
        {
            var Students = await _StudentService.GetAllStudentsAsync();
            if (Students == null || !Students.Any())
            {
               return Ok(new List<UserDto>());   
            }
             return Ok(Students);
        }

        [HttpGet("GetById/{studentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> GetStudentById(string studentId)
        {
            try
            {
                var student = await _StudentService.GetStudentByIdAsync(studentId);
                return Ok(student);
            }

            catch (ValidationException ex)
            {
                return NotFound(new { message = ex.Message });
            }

            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("Update/{studentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStudentAsync(string studentId, [FromBody] UserDto studentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updated = await _StudentService.UpdateStudentAsync(studentId, studentDto);
                return Ok(new { message = "Student updated successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("delete/{studentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStudentAsync(string studentId)
        {
            try
            {
                var deleted = await _StudentService.DeleteStudentAsync(studentId);
                return Ok(new { message = "Student deleted successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("RegisterToCourse")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> RegisterToCourse([FromBody] RegisterUserToCourseDto model)
        {
            var userId = User.GetUserId();
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
                var success = await _StudentService.RegisterToCourseAsync(userId, model.CourseCode);
                return Ok(new { message = "Student added to the course successfully." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("AllCoursestoStudent")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<CourseUserDto>> AllCoursestoStudent()
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }

            try
            {
                var courses = await _StudentService.GetAllCourseToSpecificStudentAsync(userId);
                return Ok(courses);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    } 
}

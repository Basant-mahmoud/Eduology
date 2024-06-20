using Eduology.Application.Services.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _StudentService;
        public StudentController(IStudentService studentService)
        {
            _StudentService = studentService;
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetStudents()
        {
            var Students = await _StudentService.GetAllStudentsAsync();
            if (Students == null || !Students.Any())
            {
               return Ok(new List<UserDto>());   
            }
            else
            {
                return Ok(Students);
            }
          
        }
        [HttpGet("GetById/{studentId}")]
        public async Task<ActionResult<UserDto>> GetStudentById(string studentId)
        {
            var student = await _StudentService.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }
        [HttpPut("Update/{studentId}")]
        public async Task<IActionResult> UpdateStudentAsync(string studentId, [FromBody] UserDto studentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _StudentService.UpdateStudentAsync(studentId, studentDto);
            if (!updated)
            {
                return NotFound();
            }

            return Ok(new { message = "Student updated successfully" });
        }

        [HttpDelete("delete/{studentId}")]
        public async Task<IActionResult> DeleteStudentAsync(string studentId)
        {
            var student = await _StudentService.DeleteStudentAsync(studentId);
            if (!student)
            {
                return NotFound(); 
            }

            return Ok(new { message = "Student deleted successfully" });
        }
        [HttpPost("RegisterToCourse")]
        public async Task<IActionResult> RegisterToCourse([FromBody] RegisterStudentToCourseDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _StudentService.RegisterToCourseAsync(model.StudentId, model.CourseCode);
            if (success)
                return Ok("Student added to the course successfully.");
            else
                return BadRequest("Failed to add student to the course.");
        }
    } 
}

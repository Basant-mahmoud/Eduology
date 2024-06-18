﻿using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _StudentRepository;
        public StudentController(IStudentRepository studentRepository)
        {
            _StudentRepository = studentRepository;
        }
        [HttpGet("GetAllStudents")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetStudents()
        {
            var Students = await _StudentRepository.GetAllStudentsAsync();
            if (Students == null || !Students.Any())
            {
                return NotFound();
            }
            return Ok(Students);
        }
        [HttpGet("GetStudentbyId/{studentId}")]
        public async Task<ActionResult<UserDto>> GetStudentById(string studentId)
        {
            var student = await _StudentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }
        [HttpPut("UpdateStudent/{studentId}")]
        public async Task<IActionResult> UpdateStudentAsync(string studentId, [FromBody] UserDto studentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _StudentRepository.UpdateStudentAsync(studentId, studentDto);
            if (!updated)
            {
                return NotFound();
            }

            return Ok(new { message = "Student updated successfully" });
        }

        [HttpDelete("deleteStudent/{studentId}")]
        public async Task<IActionResult> DeleteStudentAsync(string studentId)
        {
            var student = await _StudentRepository.DeleteStudentAsync(studentId);
            if (!student)
            {
                return NotFound(); 
            }

            return Ok(new { message = "Student deleted successfully" });
        }
    } 
}
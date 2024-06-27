using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Security.Claims;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : Controller
    {
        private readonly IAsignmentServices _asignmentServices;
        private readonly IWebHostEnvironment _environment;

        public AssignmentController(IAsignmentServices asignmentServices, IWebHostEnvironment environment)
        {
            _asignmentServices = asignmentServices;
            _environment = environment;
        }
        [Authorize(Roles = "Instructor")]
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromForm] AssignmentCreationDto assignment)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return Unauthorized("User ID not found in the token");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (assignment.File == null || assignment.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var uploadsPath = Path.Combine(_environment.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var filePath = Path.Combine(uploadsPath, assignment.File.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await assignment.File.CopyToAsync(stream);
            }
            var __assignment = new AssignmentDto
            {
                CourseId = assignment.CourseId,
                Deadline = assignment.Deadline,
                Description = assignment.Description,
                File = assignment.File,
                Title = assignment.Title,
                fileURLs = new AssignmentFileDto
                {
                    URL = filePath,
                    Title = assignment.File.FileName
                },

            };


            var _assignment = await _asignmentServices.CreateAsync(__assignment, userId);

            return CreatedAtAction(nameof(GetById), new { id = _assignment.CourseId }, _assignment);
        }

        [Authorize(Roles = "Instructor,Student")]
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var userId = User.FindFirst("uid")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID not found in the token");
            }
            var _assignment = await _asignmentServices.GetByIdAsync(id, userId, userRole);
            if (_assignment == null)
            {
                return BadRequest(ModelState);
            }
            return Ok(_assignment);
        }
        [Authorize(Roles = "Instructor,Student")]
        [HttpGet("GetByName/{name}")]
        public async Task<ActionResult> GetByName(String name)
        {
            var userId = User.FindFirst("uid")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID not found in the token");
            }
            var _assignment = await _asignmentServices.GetByNameAsync(name, userId, userRole);
            if (_assignment == null)
            {
                return BadRequest(ModelState);
            }
            return Ok(_assignment);
        }
        [Authorize(Roles = "Instructor")]
        [HttpDelete("Delete/{assignmentId}")]
        public async Task<ActionResult> Delete(int assignmentId, [FromBody] CourseIdDto course)
        {
            var userId = User.FindFirst("uid")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID not found in the token");
            }
            try
            {
                var _assignment = await _asignmentServices.DeleteAsync(assignmentId, course.courseId, userId,userRole);
                return Ok("Assignment deleted successfully");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Instructor")]
        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(int id, UpdateAssignmemtDto assignment)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return Unauthorized("User ID not found in the token");
            }
            var _assignment = await _asignmentServices.UpdateAsync(id, assignment, userId);
            if (_assignment == null)
            {
                return BadRequest("Assignment update failed.");
            }
            return Ok(new { Message = "Assignment Updated Successfully" });
        }
    }
     
    
}

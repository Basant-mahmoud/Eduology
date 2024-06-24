using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Models;
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
        public AssignmentController(IAsignmentServices asignmentServices)
        {
            _asignmentServices = asignmentServices;
        }
        [Authorize(Roles = "Instructor")]
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] AssignmentDto assignment)
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
            var _assignment = await _asignmentServices.CreateAsync(assignment,userId);

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
            var _assignment = await _asignmentServices.GetByIdAsync(id,userId,userRole);
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
            var _assignment = await _asignmentServices.GetByNameAsync(name,userId,userRole);
            if (_assignment == null)
            {
                return BadRequest(ModelState);
            }
            return Ok(_assignment);
        }
        [Authorize(Roles = "Instructor")]
        [HttpDelete("courses/{courseId}/assignments/{assignmentId}")]
        public async Task<ActionResult> Delete(int assignmentId,string courseId)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return Unauthorized("User ID not found in the token");
            }
            var _assignment = await _asignmentServices.DeleteAsync(assignmentId,courseId,userId);
            if (_assignment == false)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { Message = "Assignment deleted Successfully" });
        }
        [Authorize(Roles = "Instructor")]
        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Udate(int id,AssignmentDto assignment)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return Unauthorized("User ID not found in the token");
            }
            var _assignment = await _asignmentServices.UpdateAsync(id,assignment,userId);
            if (!_assignment)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { Message = "Assignment Updated Successfully" });
        }
        [Authorize(Roles = "Instructor,Student")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirst("uid")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID not found in the token");
            }

            var assignments = await _asignmentServices.GetAllAsync(userId, userRole);
            if (assignments == null || !assignments.Any())
            {
                return NotFound("No assignments found or user not authorized.");
            }

            return Ok(assignments);
        }

    }
}

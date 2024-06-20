using Eduology.Application.Interface;
using Eduology.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

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
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Assignment assignment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _assignment = await _asignmentServices.CreateAsync(assignment);

            return CreatedAtAction(nameof(GetById), new { id = _assignment.CourseId }, assignment);
        }
        [HttpGet("GetById")]
        public async Task<ActionResult> GetById(int id)
        {
            var _assignment = await _asignmentServices.GetByIdAsync(id);
            if (_assignment == null)
            {
                return BadRequest(ModelState);
            }
            return Ok(_assignment);
        }
        [HttpGet("GetByName")]
        public async Task<ActionResult> GetByNmae(String name)
        {
            var _assignment = await _asignmentServices.GetByNameAsync(name);
            if (_assignment == null)
            {
                return BadRequest(ModelState);
            }
            return Ok(_assignment);
        }
        [HttpDelete("Delete")]
        public async Task<ActionResult> Delete(int id)
        {
            var _assignment = await _asignmentServices.DeleteAsync(id);
            if (_assignment == false)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { Message = "Assignment deleted Successfully" });
        }
        [HttpPut("Update")]
        public async Task<ActionResult> Udate(int id,Assignment assignment)
        {
            var _assignment = await _asignmentServices.UpdateAsync(id,assignment);
            if (_assignment == null)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { Message = "Assignment Updated Successfully" });
        }

    }
}

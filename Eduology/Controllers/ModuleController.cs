﻿using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _ModuleService;

        public ModuleController(IModuleService moduleService)
        {
            _ModuleService = moduleService;
        }
        private string GetUserId()
        {
            return User.FindFirst("uid")?.Value;
        }
        [HttpPost("AddModule")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> AddModule([FromBody] ModuleDto module)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, exists) = await _ModuleService.AddModuleAsync(userId,module);

            if (exists)
            {
                return Conflict(new { message = "Module already exists in the course." });
            }

            if (!success)
            {
                return BadRequest(new { message = "Failed to add module. The course might not exist." });
            }

            return Ok(new { message = "Module added successfully." });
        }
        [HttpPut("UpdateModule")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> UpdateModule([FromBody] UpdateModuleDto module)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            } 
            if (!ModelState.IsValid)
            { return BadRequest(ModelState); }
            var success = await _ModuleService.UpdateModuleAsync(userId, module);

            if (!success)
            {
                return NotFound(new { message = $"Module with name {module.Name}  not found in course ." });
            }

            return Ok(new { message = $"Module with ID {module.Name} update successfully." });
        }

        [HttpDelete("DeleteModule")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteModule([FromBody] ModuleDto module)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }
            if (!ModelState.IsValid)
            { return BadRequest(ModelState); }
            var success = await _ModuleService.DeleteModuleAsync(userId,module);

            if (!success)
            {
                return NotFound(new { message = $"Module with name {module.Name}  not found in course ." });
            }

            return Ok(new { message = $"Module with ID {module.Name} deleted successfully." });
        }
        
       
    }
}

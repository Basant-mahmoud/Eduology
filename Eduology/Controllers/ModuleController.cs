using Eduology.Application.Interface;
using Eduology.Application.Services.Helper;
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
        [HttpPost("AddModule")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> AddModule([FromBody] ModuleDto module)
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
                var (success, exists) = await _ModuleService.AddModuleAsync(userId, module);

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
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }
        [HttpPut("UpdateModule")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> UpdateModule([FromBody] UpdateModuleDto module)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            } 
            if (!ModelState.IsValid)
            { return BadRequest(ModelState); }
            try
            {
                var success = await _ModuleService.UpdateModuleAsync(userId, module);
                return Ok(new { message = $"Module with ID {module.Name} update successfully." });
            }
          catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

           
        }

        [HttpDelete("DeleteModule")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteModule([FromBody] ModuleDto module)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }
            if (!ModelState.IsValid)
            { return BadRequest(ModelState); }
            try
            {
                var success = await _ModuleService.DeleteModuleAsync(userId, module);
                return Ok(new { message = $"Module with ID {module.Name} deleted successfully." });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }
    }
}

using Eduology.Application.Interface;
using Eduology.Domain.DTO;
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
        public async Task<IActionResult> AddModule([FromBody] ModuleDto module)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, exists) = await _ModuleService.AddModuleAsync(module);

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
        public async Task<IActionResult> UpdateModule([FromBody] UpdateModuleDto module)
        {
            var success = await _ModuleService.UpdateModuleAsync(module);

            if (!success)
            {
                return NotFound(new { message = $"Module with name {module.Name}  not found in course ." });
            }

            return Ok(new { message = $"Module with ID {module.Name} update successfully." });
        }

        [HttpDelete("DeleteModule")]
        public async Task<IActionResult> DeleteModule([FromBody] ModuleDto module)
        {
            var success = await _ModuleService.DeleteModuleAsync(module);

            if (!success)
            {
                return NotFound(new { message = $"Module with name {module.Name}  not found in course ." });
            }

            return Ok(new { message = $"Module with ID {module.Name} deleted successfully." });
        }
    }
}

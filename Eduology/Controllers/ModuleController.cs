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
        [HttpPost("AddType")]
        public async Task<IActionResult> AddType([FromBody] MaterialType type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, exists, createdType) = await _ModuleService.AddModuleAsync(type);
            if (exists)
            {
                return NotFound(new { message = "Module already exists." });
            }

            if (!success)
            {
                return NotFound(new { message = $"Failed to add {type} module." });
            }

            return Ok(new { message = "Module added successfully."});
        }
        [HttpGet("ModuleWithFiles/{courseId}")]
        public async Task<IActionResult> AllModuleWithFilesByCourseId(string courseId)
        {
            var typesWithFiles = await _ModuleService.GetAllModulesAsync(courseId);

            if (typesWithFiles == null || !typesWithFiles.Any())
            {
                return NotFound(new { message = "course id not found or no module found " });
            }

            return Ok(typesWithFiles);
        }
        [HttpDelete("DeleteModule/{moduleName}")]
        public async Task<IActionResult> DeleteModule(string moduleName)
        {
            var response = await _ModuleService.DeleteModule(moduleName);

            if (response == null)
            {
                return NotFound(new { message = "Failed to delete Module or Module does not exist." });
            }

            return Ok(new { message = "Module deleted successfully." });
        }
    }
}

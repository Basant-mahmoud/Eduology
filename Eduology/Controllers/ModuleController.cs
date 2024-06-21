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

            var (success, exists, createdType) = await _ModuleService.AddTypeAsync(type);
            if (exists)
            {
                return BadRequest(new { message = "Module already exists." });
            }

            if (!success)
            {
                return BadRequest(new { message = "Failed to add module." });
            }

            return Ok(new { message = "Module added successfully.", createdType });
        }
        [HttpGet("ModuleWithFiles/{courseId}")]
        public async Task<IActionResult> AllModuleWithFilesByCourseId(string courseId)
        {
            var typesWithFiles = await _ModuleService.GetModulesWithFilesAsync(courseId);

            if (typesWithFiles == null || !typesWithFiles.Any())
            {
                return NoContent();
            }

            return Ok(typesWithFiles);
        }
        [HttpDelete("DeleteModule/{moduleName}")]
        public async Task<IActionResult> DeleteModule(string moduleName)
        {
            var response = await _ModuleService.DeleteModule(moduleName);

            if (response == null)
            {
                return BadRequest(new { message = "Failed to delete Module or Module does not exist." });
            }

            return Ok(new { message = "Module deleted successfully." });
        }
    }
}

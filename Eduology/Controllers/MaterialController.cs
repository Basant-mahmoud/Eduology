using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }
        [HttpPost("AddMaterial")]
        public async Task<IActionResult> AddMaterial([FromBody] MaterialDto materialDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _materialService.AddMaterialAsync(materialDto);
            if (!success)
            {
                return BadRequest(new { message = "Failed to add material." });
            }

            return Ok(new { message = "Material added successfully." });
        }

        [HttpPost("AddType")]
        public async Task<IActionResult> AddType([FromBody] MaterialType type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, exists, createdType) = await _materialService.AddTypeAsync(type);
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
        [HttpGet("GetAllMaterial/{courseId}")]
        public async Task<IActionResult> GetAllMaterial(string courseId)
        {
            var materials = await _materialService.GetAllMaterialsAsync(courseId);

            if (materials == null || !materials.Any())
            {
                return NoContent();
            }

            return Ok(materials);
        }
        [HttpGet("ModuleWithFiles/{courseId}")]
        public async Task<IActionResult> AllModuleWithFilesByCourseId(string courseId)
        {
            var typesWithFiles = await _materialService.GetModulesWithFilesAsync(courseId);

            if (typesWithFiles == null || !typesWithFiles.Any())
            {
                return NoContent();
            }

            return Ok(typesWithFiles);
        }
        [HttpDelete("DeleteFile")]
        public async Task<IActionResult> DeleteFile([FromBody] DeleteFileDto file)
        {
            var response = await _materialService.DeleteFileAsync(file.fileId,file.courseId,file.materialType);

            if (response == null)
            {
                return BadRequest(new { message = "Failed to delete file or file does not exist." });
            }

             return Ok(new { message = "File deleted successfully." });
        }
        [HttpDelete("DeleteModule/{moduleName}")]
        public async Task<IActionResult> DeleteModule(string moduleName)
        {
            var response = await _materialService.DeleteModule(moduleName);

            if (response == null)
            {
                return BadRequest(new { message = "Failed to delete Module or Module does not exist." });
            }

            return Ok(new { message = "Module deleted successfully." });
        }
    }
}

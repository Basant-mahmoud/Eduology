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
                return NotFound(new { message = "Failed to add material." });
            }

            return Ok(new { message = "Material added successfully." });
        }

        
        [HttpGet("GetAllMaterial/{courseId}")]
        public async Task<IActionResult> GetAllMaterial(string courseId)
        {
            var materials = await _materialService.GetAllMaterialsAsync(courseId);

            if (materials == null || !materials.Any())
            {
                return NotFound(new { message = "No martials found." });
            }

            return Ok(materials);
        }
       
        [HttpDelete("DeleteFile")]
        public async Task<IActionResult> DeleteFile([FromBody] DeleteFileDto file)
        {
            var response = await _materialService.DeleteMatrialAsync(file.fileId,file.courseId,file.materialType);

            if (response == null)
            {
                return NotFound(new { message = "Failed to delete file or file does not exist." });
            }

             return Ok(new { message = "File deleted successfully." });
        }
        
    }
}

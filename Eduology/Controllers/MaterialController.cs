using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> AddMaterial([FromBody] MaterialDto materialDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _materialService.AddMaterialAsync(materialDto);
            if (!success)
            {
                return NotFound(new { message = "Failed to add material  input is not correct." });
            }

            return Ok(new { message = "Material added successfully." });
        }


        [HttpPost("GetmaterialsToInstructor")]
        [Authorize(Roles = "Instructor")]
        public async Task<ActionResult<List<GetMaterialDto>>> GetmaterialsToInstructor([FromBody] CourseInstructorRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _materialService.GetMaterialToInstructorsAsync(requestDto);
            if (result == null)
            {
                return BadRequest("Failed to retrieve modules and materials.");
            }

            return Ok(result);
        }
        [HttpPost("GetmaterialsToStudent")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<List<GetMaterialDto>>> GetmaterialsToStudent([FromBody] CourseStudentRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _materialService.GetMaterialToStudentAsync(requestDto);
            if (result == null)
            {
                return BadRequest("Failed to retrieve modules and materials.");
            }

            return Ok(result);
        }

        [HttpDelete("DeleteFile")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteFile([FromBody] DeleteFileDto file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _materialService.DeleteFileAsync(file);
            if (!success)
            {
                return NotFound($"File with Id {file.fileId} not found in  Module {file.Module}.");
            }

            return Ok("File deleted successfully."); 
        }


    }
}

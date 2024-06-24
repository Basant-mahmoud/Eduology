using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _materialService.AddMaterialAsync(userId, materialDto);
            if (!success)
            {
                return NotFound(new { message = "Failed to add material  input is not correct." });
            }

            return Ok(new { message = "Material added successfully." });
        }


        [HttpPost("GetmaterialsToInstructor")]
        [Authorize(Roles = "Instructor")]
        public async Task<ActionResult<List<GetMaterialDto>>> GetmaterialsToInstructor([FromBody] CourseUserRequestDto requestDto)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _materialService.GetMaterialToInstructorsAsync(userId, requestDto);
            if (result == null)
            {
                return BadRequest(new { message = "Failed to retrieve modules and materials." });
            }

            return Ok(result);
        }
        [HttpPost("GetmaterialsToStudent")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<List<GetMaterialDto>>> GetmaterialsToStudent([FromBody] CourseUserRequestDto requestDto)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _materialService.GetMaterialToStudentAsync(userId,requestDto);
            if (result == null)
            {
                return BadRequest(new { message = "Failed to retrieve modules and materials." });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteFile")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteFile([FromBody] DeleteFileDto file)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _materialService.DeleteFileAsync(userId,file);
            if (!success)
            {
                return NotFound(new { message = $"File with Id {file.fileId} not found in  Module {file.Module}." });
            }

            return Ok(new { message = "File deleted successfully." }); 
        }


    }
}

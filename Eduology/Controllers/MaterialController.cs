using Eduology.Application.Interface;
using Eduology.Application.Services;
using Eduology.Domain.DTO;
using Eduology.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;
      //  private readonly IWebHostEnvironment _webHostEnvironment;

        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
          
           // _webHostEnvironment = webHostEnvironment;
        }

        private string GetUserId()
        {
            return User.FindFirst("uid")?.Value;
        }

        [HttpPost("AddMaterial")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> AddMaterial([FromBody] MaterialDto materialDto)
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

            var success = await _materialService.AddMaterialAsync(userId, materialDto);
            if (!success)
            {
                return NotFound(new { message = "Failed to add material. Input is not correct." });
            }

            return Ok(new { message = "Material added successfully." });
        }

        [HttpPost("GetMaterialsToInstructor")]
        [Authorize(Roles = "Instructor")]
        public async Task<ActionResult<List<GetMaterialDto>>> GetMaterialsToInstructor([FromBody] CourseUserRequestDto requestDto)
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
            var result = await _materialService.GetMaterialToInstructorsAsync(userId, requestDto);
            if (result == null)
            {
                return BadRequest(new { message = "Failed to retrieve modules and materials." });
            }

            return Ok(result);
        }

        [HttpPost("GetMaterialsToStudent")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<List<GetMaterialDto>>> GetMaterialsToStudent([FromBody] CourseUserRequestDto requestDto)
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
            var result = await _materialService.GetMaterialToStudentAsync(userId, requestDto);
            if (result == null)
            {
                return BadRequest(new { message = "Failed to retrieve modules and materials." });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteFile")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteFile([FromBody] DeleteFileDto deleteFileDto)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return Unauthorized("User ID not found in the token");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _materialService.DeleteFileAsync(userId, deleteFileDto);
            if (!success)
            {
                return NotFound(new { message = $"File with Id {deleteFileDto.fileId} not found in Module {deleteFileDto.Module}." });
            }

            return Ok(new { message = "File deleted successfully." });
        }

    }
}

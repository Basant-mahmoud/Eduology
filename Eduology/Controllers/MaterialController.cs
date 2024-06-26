using Eduology.Application.Interface;
using Eduology.Application.Services;
using Eduology.Domain.DTO;
using Eduology.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Eduology.Domain.Models;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public MaterialController(IMaterialService materialService, IWebHostEnvironment webHostEnvironment)
        {
            _materialService = materialService;

             _webHostEnvironment = webHostEnvironment;
        }
        private string GetUserId()
        {
            return User.FindFirst("uid")?.Value;
        }

        [HttpPost("AddMaterial")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> AddMaterial([FromForm] MaterialDto materialDto)
        {
            try
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

                if (materialDto.File == null || materialDto.File.Length == 0)
                {
                    return BadRequest(new { message = "No file uploaded" });
                }

                var uploadsPath = Path.Combine(_webHostEnvironment.ContentRootPath, "uploads");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                var filePath = Path.Combine(uploadsPath, materialDto.File.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await materialDto.File.CopyToAsync(stream);
                }

                var fileUrl = Path.Combine(filePath, materialDto.File.FileName);
                var fileDto = new FileDto { Title = materialDto.File.FileName, URL = fileUrl };
                var success = await _materialService.AddMaterialAsync(userId, materialDto, fileDto);

                if (!success)
                {
                    return NotFound(new { message = "Failed to add material. Input is not correct." });
                }

                return Ok(new { message = "File uploaded and material added successfully", fileUrl });
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
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

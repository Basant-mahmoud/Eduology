using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;
        private readonly IWebHostEnvironment _environment;

        public SubmissionController(ISubmissionService submissionService, IWebHostEnvironment environment)
        {
            _submissionService = submissionService;
            _environment = environment;
        }
        [Authorize(Roles = "Instructor")]
        [HttpPost("GetById/{submistionId}")]
        public async Task<IActionResult> GetById(int submistionId, [FromBody] CourseIdDto cors)
        {
            var userId = User.FindFirst("uid")?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userId == null)
            {
                return Unauthorized(new { Message = "User ID not found in the token" });
            }
            try
            {
                var submission = await _submissionService.GetByIdAsync(submistionId, userId, cors.courseId);
                return Ok(submission);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
           
        }
        [Authorize(Roles = "Student")]
        [HttpPost("Submit")]
        public async Task<IActionResult> SubmitAsync([FromForm] SubmissionDto submissionDto)
        {
            var userId = User.FindFirst("uid")?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userId == null)
            {
                return Unauthorized(new { Message = "User ID not found in the token" });
            }
            if (submissionDto == null)
                return BadRequest(new { Message = "Submission data is null." });
            var uploadsPath = Path.Combine(_environment.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var filePath = Path.Combine(uploadsPath, submissionDto.file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await submissionDto.file.CopyToAsync(stream);
            }
            try
            {
                var _submissionDto = new submissionDetailsDto
                {
                    URL = filePath,
                    Title = submissionDto.file.FileName,
                    AssignmentId = submissionDto.AssignmentId,
                    courseId = submissionDto.courseId,
                    TimeStamp = DateTime.Now,
                    file = submissionDto.file
                };
                var submission = await _submissionService.CreateAsync(_submissionDto,userId,role);
                return Ok(submission);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"{ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }
        [Authorize(Roles = "Student")]
        [HttpDelete("DeleteSubmission")]
        public async Task<IActionResult> DeleteSubmission([FromBody] DeleteSubmissionDto deleteSubmissionDto)
        {
            var userId = User.FindFirst("uid")?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userId == null)
            {
                return Unauthorized(new {Message = "User ID not found in the token"});
            }
            if (deleteSubmissionDto == null)
                return BadRequest(new { Message = "Submission data is null." });

            try
            {
                var deletesubmission = await _submissionService.DeleteAsync(deleteSubmissionDto, userId, role);
                return Ok(new { Message = "Submission deleted successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"{ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }
        [Authorize(Roles = "Instructor")]
        [HttpPost("GetAllSubmission")]
        public async Task<IActionResult> GetAllSubmission([FromBody] GetAllSubmisionDto getSubmissionDto)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return Unauthorized(new { Message = "User ID not found in the token" });
            }
            if (getSubmissionDto == null)
            {
                return BadRequest(new { message = "Submission data is null." });
            }

            var submissions = await _submissionService.GetAllSubmission(userId, getSubmissionDto);
            if (submissions == null || !submissions.Any())
            {
                return NotFound(new { message = "No submissions found for the given course and assignment." });
            }
            return Ok(submissions);
        }
    }
}

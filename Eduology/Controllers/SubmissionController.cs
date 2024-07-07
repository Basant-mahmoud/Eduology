using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Eduology.Application.Interface;
using Eduology.Application.Services.Helper;
using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using System.Diagnostics;
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
        private readonly IConfiguration _configuration;
        public SubmissionController(ISubmissionService submissionService, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _submissionService = submissionService;
            _environment = environment;
            _configuration = configuration;
        }
        [Authorize(Roles = "Instructor")]
        [HttpPost("GetById/{submistionId}")]
        public async Task<IActionResult> GetById(int submistionId, [FromBody] courseIdDto cors)
        {
            var userId = User.GetUserId();
            var role = User.GetUserRole();
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
                return BadRequest(new { message = ex.Message });
            }

        }
        [Authorize(Roles = "Student")]
        [HttpPost("Submit")]
        public async Task<IActionResult> SubmitAsync([FromForm] SubmissionDto submissionDto)
        {
            var userId = User.GetUserId();
            var role = User.GetUserRole();
            if (userId == null)
            {
                return Unauthorized(new { Message = "User ID not found in the token" });
            }
            if (submissionDto == null)
                return BadRequest(new { Message = "Submission data is null." });
            var connectionString = _configuration.GetConnectionString("AzureBlobStorage");
            var containerName = "uploads";
            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var fileName = Guid.NewGuid() + Path.GetExtension(submissionDto.file.FileName);
            var blobClient = blobContainerClient.GetBlobClient(fileName);

            using (var stream = submissionDto.file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = submissionDto.file.ContentType });
            }

            var fileUrl = blobClient.Uri.AbsoluteUri;
            var fileDto = new FileDto { Title = submissionDto.file.FileName, URL = fileUrl };
            try
            {
                var _submissionDto = new submissionDetailsDto
                {
                    URL = fileUrl,
                    Title = submissionDto.file.FileName,
                    AssignmentId = submissionDto.AssignmentId,
                    courseId = submissionDto.courseId,
                    TimeStamp = DateTime.Now,
                    file = submissionDto.file
                };
                var submission = await _submissionService.CreateAsync(_submissionDto, userId, role);
                return Ok(submission);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Student")]
        [HttpDelete("DeleteSubmission")]
        public async Task<IActionResult> DeleteSubmission([FromBody] DeleteSubmissionDto deleteSubmissionDto)
        {
            var userId = User.GetUserId();
            var role = User.GetUserRole();
            if (userId == null)
            {
                return Unauthorized(new { Message = "User ID not found in the token" });
            }
            if (deleteSubmissionDto == null)
                return BadRequest(new { Message = "Submission data is null." });

            try
            {
                var deletesubmission = await _submissionService.DeleteAsync(deleteSubmissionDto, userId, role);

                if (deletesubmission)
                {
                    return Ok(new { Message = "Submission deleted successfully" });
                }
                else
                {
                    return BadRequest(new { Message = "Failed to delete submission." });
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Instructor")]
        [HttpPost("GetAllSubmission")]
        public async Task<IActionResult> GetAllSubmission([FromBody] GetAllSubmisionDto getSubmissionDto)
        {
            var userId = User.GetUserId();
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
        [Authorize(Roles = "Student")]
        [HttpPost("IsSubmited")]
        public async Task<IActionResult> IsSubmited([FromBody] IsSubmissionExistDto submissionExistDto)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { Message = "User ID not found in the token" });
            }
            if (submissionExistDto == null)
            {
                return BadRequest(new { Message = "Submission data is null." });
            }

            try
            {
                bool isSubmit = await _submissionService.IsThereSubmissionByStudentAndAssignmentAsync(submissionExistDto, userId);
                if (!isSubmit)
                {
                    return Ok(new { Message = "No submissions, you can submit" });
                }
                return Ok(new { Message = "You have already submitted" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Instructor")]
        [HttpPost("AssignGrade/{submissionId}")]
        public async Task<IActionResult> AssignGrade(int submissionId, [FromBody] GradeDto gradeDto)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { Message = "User ID not found in the token" });
            }

            try
            {
                bool result = await _submissionService.AssignGradeAsync(submissionId, gradeDto.Grade, userId, gradeDto.courseid);
                if (!result)
                {
                    return BadRequest(new { Message = "Failed to assign grade." });
                }
                return Ok(new { Message = "Grade assigned successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Student")]
        [HttpGet("GetAllGrades")]
        public async Task<IActionResult> GetAllGrades()
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { Message = "User ID not found in the token" });
            }

            try
            {
                var grades = await _submissionService.GetAllGradesAsync(userId);
                if (grades == null || !grades.Any())
                {
                    return NotFound(new { Message = "No grades found for the specified student." });
                }
                return Ok(grades);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
    }
}
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Eduology.Application.Interface;
using Eduology.Application.Services.Helper;
using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Security.Claims;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : Controller
    {
        private readonly IAsignmentServices _asignmentServices;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        public AssignmentController(IAsignmentServices asignmentServices, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _asignmentServices = asignmentServices;
            _environment = environment;
            _configuration = configuration;
        }
        [Authorize(Roles = "Instructor")]
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromForm] AssignmentCreationDto assignment)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { Message = "User ID not found in the token" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (assignment.File == null || assignment.File.Length == 0)
            {
                return BadRequest(new { Message = "No file uploaded." });
            }

            var connectionString = _configuration.GetConnectionString("AzureBlobStorage");
            var containerName = "uploads";
            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var fileName = Guid.NewGuid() + Path.GetExtension(assignment.File.FileName);
            var blobClient = blobContainerClient.GetBlobClient(fileName);

            using (var stream = assignment.File.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = assignment.File.ContentType });
            }

            var fileUrl = blobClient.Uri.AbsoluteUri;
            var fileDto = new FileDto { Title = assignment.File.FileName, URL = fileUrl };

            var __assignment = new AssignmentDto
            {
                CourseId = assignment.CourseId,
                Deadline = assignment.Deadline,
                Description = assignment.Description,
                File = assignment.File,
                Title = assignment.Title,
                fileURLs = new AssignmentFileDto
                {
                    URL = fileUrl,
                    Title = assignment.File.FileName
                },

            };
            try
            {
                var _assignment = await _asignmentServices.CreateAsync(__assignment, userId);

                return CreatedAtAction(nameof(GetById), new { id = _assignment.Id }, _assignment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }


        }

        [Authorize(Roles = "Instructor,Student")]
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var userId = User.GetUserId();
            var userRole = User.GetUserRole();

            if (userId == null)
            {
                return Unauthorized(new { Message = "User ID not found in the token" });
            }
            try
            {
                var _assignment = await _asignmentServices.GetByIdAsync(id, userId, userRole);
                if (_assignment == null)
                {
                    return BadRequest(ModelState);
                }
                return Ok(_assignment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }


        }
        [Authorize(Roles = "Instructor,Student")]
        [HttpGet("GetByName/{name}")]
        public async Task<ActionResult> GetByName(String name)
        {
            var userId = User.GetUserId();
            var userRole = User.GetUserRole();

            if (userId == null)
            {
                return Unauthorized(new { Message = "User ID not found in the token" });
            }
            try
            {
                var _assignment = await _asignmentServices.GetByNameAsync(name, userId, userRole);
                if (_assignment == null)
                {
                    return BadRequest(ModelState);
                }
                return Ok(_assignment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
        [Authorize(Roles = "Instructor")]
        [HttpDelete("Delete/{assignmentId}")]
        public async Task<ActionResult> Delete(int assignmentId, [FromBody] courseIdDto course)
        {
            var userId = User.GetUserId();
            var userRole = User.GetUserRole();

            if (userId == null)
            {
                return Unauthorized(new { Message = "User ID not found in the token" });
            }
            try
            {
                var _assignment = await _asignmentServices.DeleteAsync(assignmentId, course.courseId, userId,userRole);
                return Ok(new {Message = "Assignment deleted successfully" });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Instructor")]
        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(int id, UpdateAssignmemtDto assignment)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { Message = "User ID not found in the token" });
            }
            try
            {
                var _assignment = await _asignmentServices.UpdateAsync(id, assignment, userId);
                if (_assignment == null)
                {
                    return BadRequest(new { Message = "Assignment update failed." });
                }
                return Ok(new { Message = "Assignment Updated Successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
     
    
}

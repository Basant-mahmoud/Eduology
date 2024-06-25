﻿using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;

        public SubmissionController(ISubmissionService submissionService)
        {
            _submissionService = submissionService;
        }
        [Authorize(Roles = "Instructor")]
        [HttpGet("courses/{courseId}/Submissions/GetById/{submistionId}")]
        public async Task<IActionResult> GetById(string courseId, int submistionId)
        {
            var userId = User.FindFirst("uid")?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userId == null)
            {
                return Unauthorized("User ID not found in the token");
            }
            var submission = await _submissionService.GetByIdAsync(submistionId, userId, courseId);
            if (submission == null)
                return NotFound();
            return Ok(submission);
        }
        [Authorize(Roles = "Student")]
        [HttpPost("Submit")]
        public async Task<IActionResult> SubmitAsync([FromBody] SubmissionDto submissionDto)
        {
            var userId = User.FindFirst("uid")?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userId == null)
            {
                return Unauthorized("User ID not found in the token");
            }
            if (submissionDto == null)
                return BadRequest("Submission data is null.");

            try
            {
                var submission = await _submissionService.CreateAsync(submissionDto,userId,role);
                return Ok(submission);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Instructor")]
        [HttpDelete("DeleteSubmission")]
        public async Task<IActionResult> DeleteSubmission([FromBody] DeleteSubmissionDto deleteSubmissionDto)
        {
            var userId = User.FindFirst("uid")?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userId == null)
            {
                return Unauthorized("User ID not found in the token");
            }
            if (deleteSubmissionDto == null)
                return BadRequest("Submission data is null.");

            try
            {
                var deletesubmission = await _submissionService.DeleteAsync(deleteSubmissionDto,userId);
                return Ok(deleteSubmissionDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Instructor")]
        [HttpPost("GetAllSubmission")]
        public async Task<IActionResult> GetAllSubmission([FromBody] GetAllSubmisionDto getSubmissionDto)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return Unauthorized("User ID not found in the token");
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

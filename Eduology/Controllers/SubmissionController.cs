using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var submission = await _submissionService.GetByIdAsync(id);
            if (submission == null)
                return NotFound();
            return Ok(submission);
        }

        [HttpPost("Submit")]
        public async Task<IActionResult> SubmitAsync([FromBody] SubmissionDto submissionDto)
        {
            if (submissionDto == null)
                return BadRequest("Submission data is null.");

            try
            {
                var submission = await _submissionService.CreateAsync(submissionDto);
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
        [HttpDelete("DeleteSubmission")]
        public async Task<IActionResult> DeleteSubmission([FromBody] DeleteSubmissionDto deleteSubmissionDto)
        {
            if (deleteSubmissionDto == null)
                return BadRequest("Submission data is null.");

            try
            {
                var deletesubmission = await _submissionService.DeleteAsync(deleteSubmissionDto);
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
    }
}

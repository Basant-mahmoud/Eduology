using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Instructor")]
        public async Task<ActionResult<CreateAnnoncementDto>> PostAnnouncement([FromBody] CreateAnnoncementDto announcementDto)
        {
            var createdAnnouncement = await _announcementService.CreateAsync(announcementDto);
            if (createdAnnouncement == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetAnnouncement), new { id = createdAnnouncement.Id }, createdAnnouncement);
        }
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<AnnouncementDto>> GetAnnouncement(int id)
        {
            var announcement = await _announcementService.GetByIdAsync(id);
            if (announcement == null)
            {
                return NotFound(new { message = $"Announcement with id {id} not found." });
            }
            return Ok(announcement);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            var announcement = await _announcementService.GetByIdAsync(id);
            if (announcement == null)
            {
                return NotFound(new { message = $"Announcement with id {id} not found." });
            }

            await _announcementService.DeleteAsync(id);
            return Ok(new { message = "Announcement deleted successfully." });
        }
        [HttpGet("GetWithCourse/{courseId}")]
        public async Task<ActionResult<IEnumerable<AnnouncementDto>>> GetAnnouncementsByCourseId(string courseId)
        {
            if (string.IsNullOrWhiteSpace(courseId))
            {
                return BadRequest(new { message = "Course ID cannot be null or empty." });
            }
            var announcements = await _announcementService.GetAnnouncementsByCourseIdAsync(courseId);
            if (announcements == null || !announcements.Any())
            {
                return NotFound(new { message = $"Course with ID {courseId} not found." });
            }
            return Ok(announcements);
        }
        [HttpGet("GetWithCourse/{courseId}/AnnouncementID/{announcementId}")]
        public async Task<ActionResult<AnnouncementDto>> GetAnnouncementByIdAndCourseId(string courseId, int announcementId)
        {
            var announcement = await _announcementService.GetAnnouncementByIdAndCourseIdAsync(courseId, announcementId);
            if (announcement == null)
            {
                return NotFound(new { message = $"Announcement id {announcementId} or course id  {courseId} not found." });
            }
            return Ok(announcement);
        }
        [HttpGet("GetAllAnnouncementsToStudent/{studentid}")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<IEnumerable<AllAnnoncemetDto>>> GetAllStudentAnnouncement(string studentid)
        {
            var announcement = await _announcementService.GetAllAnnouncementsForStudentAsync(studentid);
            if (announcement == null)
            {
                return NotFound(new { message = $"Student with id {studentid} not found." });
            }
            return Ok(announcement);
        }
    }
}

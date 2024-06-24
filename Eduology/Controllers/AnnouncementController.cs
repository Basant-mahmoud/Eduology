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
    [Authorize]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }
        private string GetUserId()
        {
            return User.FindFirst("uid")?.Value;
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Instructor")]
        public async Task<ActionResult<CreateAnnoncementDto>> CreateAnnouncement([FromBody] CreateAnnoncementDto announcementDto)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }
            var createdAnnouncement = await _announcementService.CreateAsync(userId, announcementDto);
            if (createdAnnouncement == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetAnnouncement), new { announcemmentId = createdAnnouncement.Id, courseId = createdAnnouncement.CourseId }, createdAnnouncement);
        }

        [HttpGet("GetWithCourseID/{courseId}/AnnouncementID/{announcemmentId}")]
        [Authorize(Roles = "Instructor, Student")]
        public async Task<ActionResult<AnnouncementDto>> GetAnnouncement(int announcemmentId,string courseId)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }

            var announcement = await _announcementService.GetByIdAsync(userId, announcemmentId, courseId);
            if (announcement == null)
            {
                return NotFound(new { message = $"Announcement with id {announcemmentId} or course id  {courseId} not found." });
            }
            return Ok(announcement);
        }

        [HttpDelete("DeleteWithCourseID/{courseId}/AnnouncemmentID/{announcemmentId}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteAnnouncement(int announcemmentId, string courseId)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }

            var announcement = await _announcementService.GetByIdAsync(userId, announcemmentId, courseId);
            if (announcement == null)
            {
                return NotFound(new { message = $"Announcement with id {announcemmentId} not found." });
            }

            await _announcementService.DeleteAsync(announcemmentId);
            return Ok(new { message = "Announcement deleted successfully." });
        }

        [HttpGet("GetAnnouncementsToInstructorByCourseId/{courseId}")]
        [Authorize(Roles = "Instructor")]
        public async Task<ActionResult<IEnumerable<AnnouncementDto>>> GetAnnouncementsToInstructorByCourseId(string courseId)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }
            if (string.IsNullOrWhiteSpace(courseId))
            {
                return BadRequest(new { message = "Course ID cannot be null or empty." });
            }
            var announcements = await _announcementService.GetAnnouncementsToInstructorByCourseIdAsync(userId, courseId);
            if (announcements == null || !announcements.Any())
            {
                return NotFound(new { message = $"Course with ID {courseId} not found." });
            }
            return Ok(announcements);
        }
        [HttpGet("GetAnnouncementsToStudentByCourseId/{courseId}")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<IEnumerable<AnnouncementDto>>> GetAnnouncementsToStudentByCourseId(string courseId)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }
            if (string.IsNullOrWhiteSpace(courseId))
            {
                return BadRequest(new { message = "Course ID cannot be null or empty." });
            }
            var announcements = await _announcementService.GetAnnouncementsToByStudentCourseIdAsync(userId, courseId);
            if (announcements == null || !announcements.Any())
            {
                return NotFound(new { message = $"Course with ID {courseId} not found." });
            }
            return Ok(announcements);
        }

        [HttpGet("GetAllCourseAnnouncementsToStudent")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<IEnumerable<AllAnnoncemetDto>>> GetAllCourseAnnouncementsToStudent()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }
            var announcement = await _announcementService.GetAllAnnouncementsForStudentAsync(userId);
            if (announcement == null)
            {
                return NotFound(new { message = $"Student with id {userId} not found." });
            }
            return Ok(announcement);
        }
    }
}

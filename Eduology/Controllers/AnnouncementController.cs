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
        public async Task<ActionResult<CreateAnnoncementDto>> PostAnnouncement([FromBody] CreateAnnoncementDto announcementDto)
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
            return CreatedAtAction(nameof(GetAnnouncement), new { announcemmentid = createdAnnouncement.Id, courseid = createdAnnouncement.CourseId }, createdAnnouncement);
        }

        [HttpGet("GetWithCourseID/{courseid}/AnnouncementID/{announcemmentid}")]
        [Authorize(Roles = "Instructor, Student")]
        public async Task<ActionResult<AnnouncementDto>> GetAnnouncement(int announcemmentid,string courseid)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }

            var announcement = await _announcementService.GetByIdAsync(userId, announcemmentid, courseid);
            if (announcement == null)
            {
                return NotFound(new { message = $"Announcement with id {announcemmentid} or course id  {courseid} not found." });
            }
            return Ok(announcement);
        }

        [HttpDelete("DeleteWithCourseID/{courseid}/AnnouncemmentID/{announcemmentid}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteAnnouncement(int announcemmentid, string courseid)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }

            var announcement = await _announcementService.GetByIdAsync(userId, announcemmentid, courseid);
            if (announcement == null)
            {
                return NotFound(new { message = $"Announcement with id {announcemmentid} not found." });
            }

            await _announcementService.DeleteAsync(announcemmentid);
            return Ok(new { message = "Announcement deleted successfully." });
        }

        [HttpGet("GetWithCourse/{courseId}")]
        [Authorize(Roles = "Instructor, Student")]
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

        [HttpGet("GetAllAnnouncementsToStudent/{studentid}")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<IEnumerable<AllAnnoncemetDto>>> GetAllStudentAnnouncement()
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

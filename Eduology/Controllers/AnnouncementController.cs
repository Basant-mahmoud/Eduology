using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.ComponentModel.DataAnnotations;

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

        [HttpPost("Create")]
        [Authorize(Roles = "Instructor")]
        public async Task<ActionResult<CreateAnnoncementDto>> CreateAnnouncement([FromBody] CreateAnnoncementDto announcementDto)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in the token" });
            }
            try
            {
                var createdAnnouncement = await _announcementService.CreateAsync(userId, announcementDto);
                return CreatedAtAction(nameof(GetAnnouncement), new { announcemmentId = createdAnnouncement.Id, courseId = createdAnnouncement.CourseId }, createdAnnouncement);
            }

            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }

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
            try
            {
                var announcement = await _announcementService.GetByIdAsync(userId, announcemmentId, courseId);
                return Ok(announcement);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }

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

            try
            {
                var announcement = await _announcementService.GetByIdAsync(userId, announcemmentId, courseId);
                await _announcementService.DeleteAsync(announcemmentId);
                return Ok(new { message = "Announcement deleted successfully." });
            }
            catch (Exception ex) 
            {
                return NotFound(new { message = ex.Message });
            }
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
            try
            {
                var announcements = await _announcementService.GetAnnouncementsToInstructorByCourseIdAsync(userId, courseId);
                return Ok(announcements);
            }

            catch (Exception ex) 
            {
                return NotFound(new { message = ex.Message });
            }
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
            try
            {
                var announcements = await _announcementService.GetAnnouncementsToByStudentCourseIdAsync(userId, courseId);
                return Ok(announcements);
            }

            catch(Exception ex) 
            {
                return NotFound(new { message = ex.Message });
            }
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

            try
            {
                var announcement = await _announcementService.GetAllAnnouncementsForStudentAsync(userId);
                return Ok(announcement);
            }

            catch (Exception ex) 
            {
                return NotFound(new { message = ex.Message });
            }
        }

        private string GetUserId()
        {
            return User.FindFirst("uid")?.Value;
        }
    }
}

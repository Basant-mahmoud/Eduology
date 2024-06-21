using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<CreateAnnoncementDto>> PostAnnouncement([FromBody] CreateAnnoncementDto announcementDto)
        {
            var createdAnnouncement = await _announcementService.CreateAsync(announcementDto);
            if (createdAnnouncement == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetAnnouncement), new { createdAnnouncement });
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<AnnouncementDto>>> GetAnnouncements()
        {
            var announcements = await _announcementService.GetAllAsync();
            if (announcements == null || !announcements.Any())
            {
                return NotFound("No announcements found.");
            }
            return Ok(announcements);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<AnnouncementDto>> GetAnnouncement(int id)
        {
            var announcement = await _announcementService.GetByIdAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }
            return Ok(announcement);
        }

        [HttpDelete("Delete/{id}")]
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
        [HttpGet("Course/{courseId}")]
        public async Task<ActionResult<IEnumerable<AnnouncementDto>>> GetAnnouncementsByCourseId(string courseId)
        {
            var announcements = await _announcementService.GetAnnouncementsByCourseIdAsync(courseId);
            return Ok(announcements);
        }
        [HttpGet("Course/{courseId}/Announcement/{announcementId}")]
        public async Task<ActionResult<AnnouncementDto>> GetAnnouncementByIdAndCourseId(string courseId, int announcementId)
        {
            var announcement = await _announcementService.GetAnnouncementByIdAndCourseIdAsync(courseId, announcementId);
            if (announcement == null)
            {
                return NotFound();
            }
            return Ok(announcement);
        }
        [HttpGet("GetAllStudentAnnouncement/{studentid}")]
        public async Task<ActionResult<IEnumerable<AllAnnoncemetDto>>> GetAllStudentAnnouncement(string studentid)
        {
            var announcement = await _announcementService.GetAllAnnouncementsForStudentAsync(studentid);
            if (announcement == null)
            {
                return NotFound();
            }
            return Ok(announcement);
        }
    }
}

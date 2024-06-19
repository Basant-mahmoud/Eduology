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

        // POST: api/announcements/{courseId}/create
        [HttpPost("create/{courseId}")]
        public async Task<ActionResult<AnnouncementDto>> PostAnnouncement(string courseId, string instructorId,AnnouncementDto announcementDto)
        {
            var createdAnnouncement = await _announcementService.CreateAsync(announcementDto, courseId, instructorId);
            return CreatedAtAction(nameof(GetAnnouncement), new { id = createdAnnouncement.Id }, createdAnnouncement);
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
                return NotFound();
            }

            await _announcementService.DeleteAsync(id);
            return Ok(new { message = "Announcement deleted successfully." });
        }
    }
}

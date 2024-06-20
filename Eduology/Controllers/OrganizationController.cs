using Eduology.Application.Interface;
using Eduology.Application.Utilities;
using Eduology.Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<OrganizationDto>> PostOrganization(OrganizationDto organizationDto)
        {
            if (!ValidationHelper.IsValidEmail(organizationDto.Email))
            {
                ModelState.AddModelError("Email", "Invalid email format");
                return BadRequest(ModelState);
            }
            // Validate phone number
            if (!ValidationHelper.IsValidPhoneNumber(organizationDto.Phone))
            {
                ModelState.AddModelError("Phone", "Invalid phone number format");
                return BadRequest(ModelState);
            }

            // Validate password
            if (!ValidationHelper.IsValidPassword(organizationDto.Password))
            {
                ModelState.AddModelError("Password", "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
                return BadRequest(ModelState);
            }
            // Check if password and confirm password match
            if (organizationDto.Password != organizationDto.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Password and confirm password do not match");
                return BadRequest(ModelState);
            }
            var createdOrganization = await _organizationService.CreateOrganizationAsync(organizationDto);
            if (createdOrganization == null)
            {
                ModelState.AddModelError("Faild", "Email is already exist.");
                return BadRequest(ModelState);
            }
            return Ok(new { Message = "Organization created successfully", Organization = createdOrganization });
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<OrganizationDto>>> GetOrganizations()
        {
            var organizations = await _organizationService.GetAllOrganizationsAsync();
            return Ok(organizations);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<OrganizationDto>> GetOrganization(int id)
        {
            var organization = await _organizationService.GetOrganizationByIdAsync(id);
            if (organization == null)
            {
                return NotFound();
            }
            return Ok(organization);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteOrganization(int id)
        {
            var organization = await _organizationService.GetOrganizationByIdAsync(id);
            if (organization == null)
            {
                return NotFound();
            }

            await _organizationService.DeleteOrganizationAsync(id);
            return Ok("Organization deleted successfully.");
        }
    }
}

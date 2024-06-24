using Eduology.Application.Interface;
using Eduology.Application.Utilities;
using Eduology.Domain.DTO;
using Eduology.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<OrganizationDto>> PostOrganization(CreateOrganizationDto createrOrganizationDto)
        {
            if (!ValidationHelper.IsValidEmail(createrOrganizationDto.Email))
            {
                ModelState.AddModelError("Email", "Invalid email format");
                return BadRequest(ModelState);
            }
            // Validate phone number
            if (!ValidationHelper.IsValidPhoneNumber(createrOrganizationDto.Phone))
            {
                ModelState.AddModelError("Phone", "Invalid phone number format");
                return BadRequest(ModelState);
            }

            // Validate password
            if (!ValidationHelper.IsValidPassword(createrOrganizationDto.Password))
            {
                ModelState.AddModelError("Password", "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
                return BadRequest(ModelState);
            }
            // Check if password and confirm password match
            if (createrOrganizationDto.Password != createrOrganizationDto.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Password and confirm password do not match");
                return BadRequest(ModelState);
            }
            var createdOrganization = await _organizationService.CreateOrganizationAsync(createrOrganizationDto);
            if (createdOrganization == null)
            {
                ModelState.AddModelError("Faild", "Email is already exist.");
                return BadRequest(ModelState);
            }
            return Created("", createdOrganization);
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

        [HttpGet("AllStudentsWithOrganization/{organizationId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserDto>>> GetStudentsByOrganizationId(int organizationId)
        {
            var students = await _organizationService.GetStudentsByOrganizationIdAsync(organizationId);
            if (students == null || !students.Any())
            {
                return Ok(new List<UserDto>());
            }
            return Ok(students);
        }
        [HttpGet("GetAllInstructorsToOrganization/{organizationId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllInstructorsToOrganization(int organizationId)
        {
            var instructors = await _organizationService.GetAllInstructorsToOrganizationAsync(organizationId);
            if (instructors == null || !instructors.Any())
            {
                return Ok(new List<UserDto>());
            }
             return Ok(instructors);
        }
    }
}

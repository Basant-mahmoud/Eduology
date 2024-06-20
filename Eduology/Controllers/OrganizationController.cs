using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            await _organizationService.CreateOrganizationAsync(organizationDto);
            return CreatedAtAction(nameof(GetOrganization), new { id = organizationDto.OrganizationID }, organizationDto);
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
            return NoContent();
        }
    }
}

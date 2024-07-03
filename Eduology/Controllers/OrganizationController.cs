using Eduology.Application.Interface;
using Eduology.Application.Utilities;
using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        private readonly ISubscriptionPlanService _subscriptionPlanService;
        private readonly IPaymentService _paymentService;

        public OrganizationController(IOrganizationService organizationService, ISubscriptionPlanService subscriptionPlanService, IPaymentService paymentService)
        {
            _organizationService = organizationService;
            _subscriptionPlanService = subscriptionPlanService;
            _paymentService = paymentService;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<OrganizationDto>> PostOrganization(CreateOrganizationDto createrOrganizationDto)
        {
            try
            {
                if (!ValidationHelper.IsValidEmail(createrOrganizationDto.Email))
                {
                    ModelState.AddModelError("Email", "Invalid email format");
                    return BadRequest(ModelState);
                }

                if (!ValidationHelper.IsValidPhoneNumber(createrOrganizationDto.Phone))
                {
                    ModelState.AddModelError("Phone", "Invalid phone number format");
                    return BadRequest(ModelState);
                }

                if (!ValidationHelper.IsValidPassword(createrOrganizationDto.Password))
                {
                    ModelState.AddModelError("Password", "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
                    return BadRequest(ModelState);
                }

                if (createrOrganizationDto.Password != createrOrganizationDto.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Password and confirm password do not match");
                    return BadRequest(ModelState);
                }

                var selectedPlan = await _subscriptionPlanService.GetSubscriptionPlanByNameAsync(createrOrganizationDto.subscribtionplan);
                if (selectedPlan == null)
                {
                    ModelState.AddModelError("subscribtionplan", "Invalid subscription plan specified");
                    return BadRequest(ModelState);
                }
   
                // Save the organization DTO and return the approval URL for redirection
                var createdOrganization = await _organizationService.CreateOrganizationAsync(createrOrganizationDto, selectedPlan.subscriptionPlanId);
                return Ok(createdOrganization);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Faild", ex.Message);
                return BadRequest(ModelState);
            }
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
            try
            {
                var organization = await _organizationService.GetOrganizationByIdAsync(id);
                return Ok(organization);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteOrganization(int id)
        {
            try
            {
                var success = await _organizationService.DeleteOrganizationAsync(id);
                return Ok(new { message = "Organization deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("AllStudentsWithOrganization/{organizationId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserDto>>> GetStudentsByOrganizationId(int organizationId)
        {
            try
            {
                var students = await _organizationService.GetStudentsByOrganizationIdAsync(organizationId);
                if (students == null || !students.Any())
                {
                    return Ok(new List<UserDto>());
                }
                return Ok(students);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("GetAllInstructorsToOrganization/{organizationId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllInstructorsToOrganization(int organizationId)
        {
            try
            {
                var instructors = await _organizationService.GetAllInstructorsToOrganizationAsync(organizationId);
                if (instructors == null || !instructors.Any())
                {
                    return Ok(new List<UserDto>());
                }
                return Ok(instructors);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}

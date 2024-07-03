
using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPlanController : ControllerBase
    {
        private readonly ISubscriptionPlanService _subscriptionPlanService;

        public SubscriptionPlanController(ISubscriptionPlanService subscriptionPlanService)
        {
            _subscriptionPlanService = subscriptionPlanService;
        }

        [HttpPost("AddSubscriptionPlan")]
        public async Task<ActionResult<SubscriptionPlan>> AddSubscriptionPlan(SubscriptionPlansDto subscriptionPlansDto)
        {
            try
            {
                var addedPlan = await _subscriptionPlanService.AddSubscriptionPlanByAsync(subscriptionPlansDto);
                return Ok(addedPlan);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("GetAllSubscriptionPlans")]
        public async Task<ActionResult<List<SubscriptionPlan>>> GetAllSubscriptionPlans()
        {
            try
            {
                var subscriptionPlans = await _subscriptionPlanService.GetAllSubscriptionPlansAsync();
                return Ok(subscriptionPlans);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("GetSubscriptionPlanByName/{name}")]
        public async Task<ActionResult<SubscriptionPlan>> GetSubscriptionPlanByName(string name)
        {
            try
            {
                var subscriptionPlan = await _subscriptionPlanService.GetSubscriptionPlanByNameAsync(name);
                if (subscriptionPlan == null)
                    return NotFound();

                return Ok(subscriptionPlan);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

}

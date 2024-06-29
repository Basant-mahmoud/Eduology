using Eduology.Infrastructure.ExternalServices.Paymnet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eduology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class paymentController : ControllerBase
    {
        private readonly PayPalService _payPalService;
        public paymentController(PayPalService payPalService)
        {
            _payPalService = payPalService;
        }
        [HttpGet("captureOrder/{orderId}")]
        public async Task<IActionResult> CaptureOrder(string orderId)
        {
            try
            {
                var captureId = await _payPalService.CaptureOrder(orderId);
                return Ok(new { captureId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

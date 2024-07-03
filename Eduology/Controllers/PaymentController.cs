using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Eduology.Application.Interface;
using Eduology.Infrastructure.External_Services.PaymentModels;
using Microsoft.Extensions.Configuration;

namespace Eduology.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymobService;
        private readonly IConfiguration _configuration;
        private readonly ISubscriptionPlanService _subscriptionPlanService;
        public PaymentController(IPaymentService paymentService, IConfiguration configuration, ISubscriptionPlanService subscriptionPlanService)
        {
            _paymobService = paymentService;
            _configuration = configuration;
            _subscriptionPlanService = subscriptionPlanService;
        }

        [HttpPost("pay/{plan}")]
        public async Task<IActionResult> Pay(string plan)
        {
                // Get auth token
                var authToken = await _paymobService.GetAuthTokenAsync();

                // Register order
                var orderId = await _paymobService.RegisterOrderAsync(authToken);

            // Generate payment key with hardcoded billing data
                var _plan = await _subscriptionPlanService.GetSubscriptionPlanByNameAsync(plan);
                var paymentKeyRequest = new PaymentKeyRequest
                {
                    AuthToken = authToken,
                    AmountCents = (int)_plan.Price,
                    Expiration = 3600,
                    OrderId = orderId,
                    Currency = "EGP",
                    IntegrationId = _configuration.GetValue<string>("Paymob:IntegrationId"),
                    LockOrderWhenPaid = false
                };
                var json = JsonSerializer.Serialize(paymentKeyRequest);
                var paymentKey = await _paymobService.GeneratePaymentKeyAsync(authToken,orderId);


                //var paymentKey = await _paymobService.GeneratePaymentKeyAsync(authToken, paymentKeyRequest);
                var iframeId = _configuration.GetValue<string>("Paymob:IframeId");

                // Redirect to Paymob payment page
                var paymentUrl = $"https://accept.paymobsolutions.com/api/acceptance/iframes/{iframeId}?payment_token={paymentKey}";

                //Redirect(paymentUrl)
                return Ok(new { paymentUrl });
            
           
        }


    }

}


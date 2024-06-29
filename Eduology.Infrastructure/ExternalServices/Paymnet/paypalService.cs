using Eduology.Infrastructure.Options;
using Microsoft.Extensions.Options;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.ExternalServices.Paymnet
{
    public class PayPalService
    {
        private readonly PayPalHttpClient _client;

        public PayPalService(IOptions<PayPalOptions> options)
        {
            var paypalOptions = options.Value;
            PayPalEnvironment environment = paypalOptions.IsProduction
                ? new LiveEnvironment(paypalOptions.ClientId, paypalOptions.ClientSecret)
                : new SandboxEnvironment(paypalOptions.ClientId, paypalOptions.ClientSecret);

            _client = new PayPalHttpClient(environment);
        }

        public async Task<string> CreateOrder(decimal amount, string currency = "USD")
        {
            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(BuildRequestBody(amount, currency));

            var response = await _client.Execute(request);
            var result = response.Result<Order>();

            return result.Id;
        }

        private OrderRequest BuildRequestBody(decimal amount, string currency)
        {
            return new OrderRequest
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = currency,
                            Value = amount.ToString("0.00")
                        }
                    }
                }
            };
        }
        public async Task<string> CaptureOrder(string orderId)
        {
            try
            {
                var request = new OrdersCaptureRequest(orderId);
                request.Prefer("return=representation");

                var response = await _client.Execute(request);
                var result = response.Result<Order>();

                return result.Id; 
            }
            catch (HttpException ex)
            {
                throw new Exception($"Failed to capture order: {ex.Message}");
            }
        }

    }
}

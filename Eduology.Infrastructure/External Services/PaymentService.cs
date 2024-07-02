using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Eduology.Application.Interface;
using Eduology.Infrastructure.External_Services.PaymentModels;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
namespace aqay_apis.Services

{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PaymentService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GetAuthTokenAsync()
        {
            var requestBody = new
            {
                api_key = _configuration["Paymob:ApiKey"]
            };

            var response = await _httpClient.PostAsJsonAsync("https://accept.paymobsolutions.com/api/auth/tokens", requestBody);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var authTokenResponse = JsonConvert.DeserializeObject<AuthTokenResponse>(content);

            return authTokenResponse.Token;
        }

        public async Task<int> RegisterOrderAsync(string authToken)

        {
            var orderRequest = new
            {
                auth_token = authToken,
                delivery_needed = false,
                amount_cents = 10000,
                currency = "EGP",
                items = new List<string>() // Add items if needed
            };
            var response = await _httpClient.PostAsJsonAsync("https://accept.paymobsolutions.com/api/ecommerce/orders", orderRequest);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var orderResponse = JsonConvert.DeserializeObject<OrderResponse>(content);

            return orderResponse.OrderId;
        }
        public async Task<string> GeneratePaymentKeyAsync(string authToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var paymentKeyRequest = new
            {
                auth_token = authToken,
                amount_cents = 100,
                expiration = 3600,
                order_id = 222376867,
                billing_data = new
                {
                    apartment = "803",
                    email = "claudette09@exa.com",
                    floor = "42",
                    first_name = "Clifford",
                    street = "Ethan Land",
                    building = "8028",
                    phone_number = "+86(8)9135210487",
                    shipping_method = "PKG",
                    postal_code = "01898",
                    city = "Jaskolskiburgh",
                    country = "CR",
                    last_name = "Nicolas",
                    state = "Utah"
                },
                currency = "EGP",
                integration_id = 4604660,
                lock_order_when_paid = false
            };

            var jsonBody = JsonConvert.SerializeObject(paymentKeyRequest);
            var requestContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://accept.paymobsolutions.com/api/acceptance/payment_keys", requestContent);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var paymentKeyResponse = JsonConvert.DeserializeObject<PaymentKeyResponse>(content);

            return paymentKeyResponse.Token;
        }


    }


}


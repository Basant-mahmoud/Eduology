using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.External_Services.PaymentModels
{
    public class OrderResponse
    {
        [JsonProperty("order_id")]
        public int OrderId { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("amount_cents")]
        public int AmountCents { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("is_payment_locked")]
        public bool IsPaymentLocked { get; set; }

        [JsonProperty("order_url")]
        public string OrderUrl { get; set; }

        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonProperty("merchant_staff_tag")]
        public object MerchantStaffTag { get; set; }

        [JsonProperty("api_source")]
        public string ApiSource { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; set; } 

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}

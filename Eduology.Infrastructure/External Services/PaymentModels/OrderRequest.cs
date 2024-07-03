using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.External_Services.PaymentModels
{
    public class OrderRequest
    {

        [JsonProperty("auth_token")]
        public string AuthToken { get; set; }

        [JsonProperty("amount_cents")]
        public int AmountCents { get; set; } = 10000;

        [JsonProperty("currency")]
        public string Currency { get; set; } = "EGP";
    }
}

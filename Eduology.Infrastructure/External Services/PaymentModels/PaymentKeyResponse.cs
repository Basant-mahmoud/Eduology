using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.External_Services.PaymentModels
{
    public class PaymentKeyResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}

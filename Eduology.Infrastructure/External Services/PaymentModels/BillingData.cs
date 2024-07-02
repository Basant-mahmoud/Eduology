using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.External_Services.PaymentModels
{
    public class BillingData
    {
        [JsonProperty("apartment")]
        public string Apartment { get; set; } = "803";

        [JsonProperty("email")]
        public string Email { get; set; } = "claudette09@exa.com";

        [JsonProperty("floor")]
        public string Floor { get; set; } = "42";

        [JsonProperty("first_name")]
        public string FirstName { get; set; } = "Clifford";

        [JsonProperty("street")]
        public string Street { get; set; } = "Ethan Land";

        [JsonProperty("building")]
        public string Building { get; set; } = "8028";

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; } = "+86(8)9135210487";

        [JsonProperty("shipping_method")]
        public string ShippingMethod { get; set; } = "PKG";

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; } = "01898";

        [JsonProperty("city")]
        public string City { get; set; } = "Jaskolskiburgh";

        [JsonProperty("country")]
        public string Country { get; set; } = "CR";

        [JsonProperty("last_name")]
        public string LastName { get; set; } = "Nicolas";

        [JsonProperty("state")]
        public string State { get; set; } = "Utah";
    }
}

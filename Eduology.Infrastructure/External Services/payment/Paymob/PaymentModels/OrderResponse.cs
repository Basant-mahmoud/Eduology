using Eduology.Infrastructure.External_Services.payment.Paymob.PaymentModels;
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
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("delivery_needed")]
        public bool DeliveryNeeded { get; set; }

        [JsonProperty("merchant")]
        public MerchantDetails Merchant { get; set; }

        [JsonProperty("collector")]
        public object Collector { get; set; } // Adjust type as per actual response

        [JsonProperty("amount_cents")]
        public int AmountCents { get; set; }

        [JsonProperty("shipping_data")]
        public object ShippingData { get; set; } // Adjust type as per actual response

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("is_payment_locked")]
        public bool IsPaymentLocked { get; set; }

        [JsonProperty("is_return")]
        public bool IsReturn { get; set; }

        [JsonProperty("is_cancel")]
        public bool IsCancel { get; set; }

        [JsonProperty("is_returned")]
        public bool IsReturned { get; set; }

        [JsonProperty("is_canceled")]
        public bool IsCanceled { get; set; }

        [JsonProperty("merchant_order_id")]
        public object MerchantOrderId { get; set; } // Adjust type as per actual response

        [JsonProperty("wallet_notification")]
        public object WalletNotification { get; set; } // Adjust type as per actual response

        [JsonProperty("paid_amount_cents")]
        public int PaidAmountCents { get; set; }

        [JsonProperty("notify_user_with_email")]
        public bool NotifyUserWithEmail { get; set; }

        [JsonProperty("items")]
        public List<object> Items { get; set; } // Adjust type as per actual response

        [JsonProperty("order_url")]
        public string OrderUrl { get; set; }

        [JsonProperty("commission_fees")]
        public int CommissionFees { get; set; }

        [JsonProperty("delivery_fees_cents")]
        public int DeliveryFeesCents { get; set; }

        [JsonProperty("delivery_vat_cents")]
        public int DeliveryVatCents { get; set; }

        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonProperty("merchant_staff_tag")]
        public object MerchantStaffTag { get; set; } // Adjust type as per actual response

        [JsonProperty("api_source")]
        public string ApiSource { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; set; } // Adjust type as per actual response

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }


}

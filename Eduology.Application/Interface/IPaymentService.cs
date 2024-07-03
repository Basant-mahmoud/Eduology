using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Application.Interface
{
    public interface IPaymentService
    {
        Task<string> GetAuthTokenAsync();
        Task<int> RegisterOrderAsync(string authToken);
        Task<string> GeneratePaymentKeyAsync(string authToken,int orderId);

    }
}

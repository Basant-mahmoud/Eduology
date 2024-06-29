using Eduology.Domain.DTO;
using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Application.Interface
{
    public interface ISubscriptionPlanService
    {
        Task<List<SubscriptionPlan>> GetAllSubscriptionPlansAsync();
        Task<SubscriptionPlan> GetSubscriptionPlanByNameAsync(string name);
        Task<SubscriptionPlan> AddSubscriptionPlanByAsync(SubscriptionPlansDto subscriptionPlansDto);
    }
}

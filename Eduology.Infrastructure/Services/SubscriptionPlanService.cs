using Eduology.Application.Interface;
using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Services
{
    public class SubscriptionPlanService : ISubscriptionPlanService
    {
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

        public SubscriptionPlanService(ISubscriptionPlanRepository subscriptionPlanRepository)
        {
            _subscriptionPlanRepository = subscriptionPlanRepository;
        }
        public async Task<SubscriptionPlan> AddSubscriptionPlanByAsync(SubscriptionPlansDto subscriptionPlansDto)
        {
         
            var addedPlan = await _subscriptionPlanRepository.AddSubscriptionPlanByAsync(subscriptionPlansDto);

            if (addedPlan == null)
            {
                throw new Exception("plan not added");
            }
            return addedPlan;
        }

        public async Task<List<SubscriptionPlan>> GetAllSubscriptionPlansAsync()
        {
            var subscriptionPlans = await _subscriptionPlanRepository.GetAllSubscriptionPlansAsync();
            if (subscriptionPlans == null)
            {
                return new List<SubscriptionPlan>();
            }
            return subscriptionPlans;
        }

        public async Task<SubscriptionPlan> GetSubscriptionPlanByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Name cannot be null or empty.");
            }

            return await _subscriptionPlanRepository.GetSubscriptionPlanByNameAsync(name);
        }

    }
}

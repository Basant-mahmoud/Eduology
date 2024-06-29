using Eduology.Domain.DTO;
using Eduology.Domain.Interfaces;
using Eduology.Domain.Models;
using Eduology.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduology.Infrastructure.Repositories
{
    public class SubscriptionPlanRepository : ISubscriptionPlanRepository
    {
        private readonly EduologyDBContext _context;
        public SubscriptionPlanRepository(EduologyDBContext context)
        {
            _context = context;
        }

        public async Task<SubscriptionPlan> AddSubscriptionPlanByAsync(SubscriptionPlansDto subscriptionPlan)
        {
            var plan = new SubscriptionPlan
            {
                Currency = subscriptionPlan.Currency,
                Name = subscriptionPlan.Name,
                Price = subscriptionPlan.Price,
            };
            var entry = await _context.SubscriptionPlans.AddAsync(plan);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<List<SubscriptionPlan>> GetAllSubscriptionPlansAsync()
        {
            return await _context.SubscriptionPlans.ToListAsync();
        }

        public async Task<SubscriptionPlan> GetSubscriptionPlanByNameAsync(string name)
        {
            var plan =  await _context.SubscriptionPlans.FirstOrDefaultAsync(sp => sp.Name == name);
            if (plan == null)
                return null;
            return plan;
        }
    }
}

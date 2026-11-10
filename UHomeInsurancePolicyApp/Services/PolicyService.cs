using Microsoft.EntityFrameworkCore;
using UHomeInsurancePolicyApp.Data;
using UHomeInsurancePolicyApp.Models;

namespace UHomeInsurancePolicyApp.Services
{
    public class PolicyService
    {
        private readonly PolicyDbContext _context;

        public PolicyService(PolicyDbContext context)
        {
            _context = context;
        }

        public async Task<Policy> SellPolicyAsync(Policy policy)
        {
            var policyExists = await _context.Policies.AnyAsync(p => p.PolicyReference == policy.PolicyReference);

            //unique reference number
            if (policyExists)
            {
                throw new InvalidOperationException(
                    "A policy with this reference already exists.");
            }
            //policy starts within 60 days
            if (policy.StartDate.Date > DateTime.Today.AddDays(60))
            {
                throw new InvalidOperationException(
                    "A policy cannot start more than 60 days in advance.");
            }

            if (policy.StartDate.Date < DateTime.Today)
            {
                throw new InvalidOperationException(
                    "A policy cannot start in the past.");
            }

            _context.Policies.Add(policy);
                
            await _context.SaveChangesAsync();

            return policy;
        }
    }
}

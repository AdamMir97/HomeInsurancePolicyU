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

            if (policyExists)
            {
                throw new InvalidOperationException(
                    "A policy with this reference already exists.");
            }
            else
            {
                _context.Policies.Add(policy);
                
            }

            await _context.SaveChangesAsync();

            return policy;
        }
    }
}

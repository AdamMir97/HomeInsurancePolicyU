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
            _context.Policies.Add(policy);

            await _context.SaveChangesAsync();

            return policy;
        }
    }
}

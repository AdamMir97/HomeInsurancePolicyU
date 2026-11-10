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
            //policy lasts for a year
            if (policy.EndDate != policy.StartDate.AddYears(1))
            {
                throw new InvalidOperationException(
                    "A policy must be exactly one year in length.");
            }
            //only 3 policy holders allowed
            if (policy.Policyholders.Count < 1 || policy.Policyholders.Count > 3)
            {
                throw new InvalidOperationException(
                    "A policy must have between 1 and 3 policyholders.");
            }
            //policy users must be above 16 (I included 16 y/os)
            foreach (var policyholder in policy.Policyholders)
            {
                var age = policy.StartDate.Year -
                          policyholder.DateOfBirth.Year;

                if (policyholder.DateOfBirth.Date > policy.StartDate.Date.AddYears(-age))
                {
                    age--;
                }

                if (age < 16)
                {
                    throw new InvalidOperationException(
                        "All policyholders must be over 16 on the policy start date.");
                }
            }
            _context.Policies.Add(policy);
                
            await _context.SaveChangesAsync();

            return policy;
        }
    }
}

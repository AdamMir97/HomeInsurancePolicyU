using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace UHomeInsurancePolicyApp.Models
{
    public class Policy
    {
        public int Id { get; set; }

        public string PolicyReference { get; set; } = string.Empty;

        public PolicyType PolicyType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Amount { get; set; }

        public bool HasClaims { get; set; }

        public bool AutoRenew { get; set; }

        public List<Policyholder> Policyholders { get; set; } = [];

        public Property Property { get; set; } = new();
    }
}

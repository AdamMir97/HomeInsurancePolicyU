using Microsoft.EntityFrameworkCore;
using UHomeInsurancePolicyApp.Models;

namespace UHomeInsurancePolicyApp.Data
{
    //can alternatively use a repository + interface to allow swapping with a database later
    public class PolicyDbContext : DbContext
    {
        public PolicyDbContext(DbContextOptions<PolicyDbContext> options)
    : base(options)
        {
        }

        public DbSet<Policy> Policies => Set<Policy>();

        public DbSet<Policyholder> Policyholders => Set<Policyholder>();

        public DbSet<Property> Properties => Set<Property>();

        public DbSet<Payment> Payments => Set<Payment>();
    }
}

using Microsoft.EntityFrameworkCore;
using UHomeInsurancePolicyApp.Data;
using UHomeInsurancePolicyApp.Models;
using UHomeInsurancePolicyApp.Services;

namespace HomeInsurancePolicyTests
{
    [TestFixture]
    public class PolicyServiceTests
    {
        private PolicyDbContext _context = null!;
        private PolicyService _service = null!;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PolicyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new PolicyDbContext(options);
            _service = new PolicyService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        // Helper method for creating valid policies
        private static Policy CreateValidPolicy()
        {
            var startDate = DateTime.Today.AddDays(10);

            return new Policy
            {
                PolicyReference = "POL001",
                PolicyType = PolicyType.Household,
                StartDate = startDate,
                EndDate = startDate.AddYears(1),
                Amount = 250m,
                Policyholders =
                [
                    new Policyholder
                {
                    FirstName = "John",
                    LastName = "Smith",
                    DateOfBirth = startDate.AddYears(-30)
                }
                ],
                Property = new Property
                {
                    AddressLine1 = "10 High Street",
                    Postcode = "M1 1AA"
                }
            };
        }

        // Saving policies
        [Test]
        public async Task SellPolicy_ValidPolicy_SavesPolicy()
        {
            // Arrange
            var policy = CreateValidPolicy();

            // Act
            await _service.SellPolicyAsync(policy);

            // Assert
            Policy? savedPolicy = await _context.Policies
                .FirstOrDefaultAsync(p =>
                    p.PolicyReference == policy.PolicyReference);

            Assert.That(savedPolicy, Is.Not.Null);
            Assert.That(savedPolicy!.PolicyReference,
                Is.EqualTo(policy.PolicyReference));
        }

        // Unique policy ref
        [Test]
        public async Task SellPolicy_DuplicateReference_ThrowsException()
        {
            // Arrange
            var firstPolicy = CreateValidPolicy();
            await _service.SellPolicyAsync(firstPolicy);

            var secondPolicy = CreateValidPolicy();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.SellPolicyAsync(secondPolicy));
        }

        // Policy must start within 60 days
        [TestCase(0, true)]
        [TestCase(30, true)]
        [TestCase(60, true)]
        [TestCase(61, false)]
        public async Task SellPolicy_StartDate_ValidatedCorrectly(int daysAhead,bool shouldSucceed)
        {
            //Arrange
            var policy = CreateValidPolicy();

            policy.StartDate = DateTime.Today.AddDays(daysAhead);
            policy.EndDate = policy.StartDate.AddYears(1);

            //Act & Assert
            if (shouldSucceed)
            {
                await _service.SellPolicyAsync(policy);
            }
            else
            {
                await Assert.ThrowsAsync<InvalidOperationException>(
                    () => _service.SellPolicyAsync(policy));
            }
        }

        // Policy must be a year long
        [TestCase(12, true)]
        [TestCase(6, false)]
        [TestCase(18, false)]
        public async Task SellPolicy_Duration_ValidatedCorrectly(int months,bool shouldSucceed)
        {
            var policy = CreateValidPolicy();
            policy.EndDate = policy.StartDate.AddMonths(months);

            if (shouldSucceed)
            {
                await _service.SellPolicyAsync(policy);
            }
            else
            {
                await Assert.ThrowsAsync<InvalidOperationException>(
                    async () => await _service.SellPolicyAsync(policy));
            }
        }
    }
}
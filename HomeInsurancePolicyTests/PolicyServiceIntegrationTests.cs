using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using UHomeInsurancePolicyApp;
using UHomeInsurancePolicyApp.Models;

namespace HomeInsurancePolicyTests;

[TestFixture]
public class PolicyServiceIntegrationTests
{
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task PostPolicy_ValidPolicy_ReturnsCreated()
    {
        // Arrange
        var request = CreateValidPolicyRequest();

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/policies", request);

        // Assert
        Assert.That(response.StatusCode,
            Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task GetPolicy_UnknownReference_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/policies/UNKNOWN");

        // Assert
        Assert.That(response.StatusCode,
            Is.EqualTo(HttpStatusCode.NotFound));
    }

    private static Policy CreateValidPolicyRequest()
    {
        var startDate = DateTime.Today.AddDays(1);

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
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UHomeInsurancePolicyApp.Models;
using UHomeInsurancePolicyApp.Services;

namespace UHomeInsurancePolicyApp.Controllers
{
    [Route("api/policies")]
    [ApiController]
    public class HomeInsurancePolicyController : ControllerBase
    {
        private readonly PolicyService _service;

        public HomeInsurancePolicyController(PolicyService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> SellPolicy(Policy policy)
        {
            try
            {
                var result = await _service.SellPolicyAsync(policy);

                return CreatedAtAction(
                    nameof(GetPolicy),
                    new { reference = result.PolicyReference },
                    result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{reference}")]
        public async Task<IActionResult> GetPolicy(string reference)
        {
            var policy = await _service.GetPolicyAsync(reference);

            if (policy == null)
            {
                return NotFound(new
                {
                    message = $"Policy {reference} was not found."
                });
            }

            return Ok(policy);
        }

    }
}

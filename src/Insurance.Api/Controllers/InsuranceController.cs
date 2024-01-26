using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.Models.Request;
using Insurance.Api.Services.Insurance;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    [ApiController]
    [Route("api/insurance")]
    public class InsuranceController : Controller
    {

        private readonly IInsuranceService _insuranceService;

        public InsuranceController(IInsuranceService insuranceService)
        {
            _insuranceService = insuranceService;
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult> CalculateInsurance([FromRoute] int productId)
        {
            var insurance = await _insuranceService.CalculateProductInsurance(productId);

            return Ok(insurance);
        }

        [HttpPost("product")]
        public async Task<ActionResult> CalculateCartInsurance(CartRequest cartRequest)
        {
            var cartInsurance = await _insuranceService.CalculateCartInsurance(cartRequest);

            return Ok(cartInsurance);
        }
    }
}
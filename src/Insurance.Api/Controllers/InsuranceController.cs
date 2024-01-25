using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    [Route("api/[controller]")]
    public class InsuranceController: Controller
    {

        private readonly IInsuranceService _insuranceService;

        public InsuranceController(IInsuranceService insuranceService)
        {
            _insuranceService = insuranceService;
        }

        [HttpGet]
        [Route("/product/{productId}")]
        public async Task<ActionResult> CalculateInsurance([FromRoute] int productId)
        {
            var insurance = await _insuranceService.CalculateInsurance(productId);

            return Ok(insurance);
        }

        [HttpPost]
        [Route("/product")]
        public async Task<ActionResult> CalculateCartInsurance(List<int> productIds)
        {
            var cartInsurance = await _insuranceService.CalculateCartInsurance(productIds);

            return Ok(cartInsurance);
        }
    }
}
using System.Threading.Tasks;
using Insurance.Api.Application.Models.Dto;
using Insurance.Api.Application.Services.Insurance;
using Insurance.Api.Presentation.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Presentation.Controllers
{
    /// <summary>
    /// This api handles all the logic for insurance.
    /// </summary> 
    [ApiController]
    [Route("api/insurance")]
    public class InsuranceController : Controller
    {

        private readonly IInsuranceService _insuranceService;

        public InsuranceController(IInsuranceService insuranceService)
        {
            _insuranceService = insuranceService;
        }

        /// <summary>
        /// Calculate the insurance cost for one product
        /// </summary>
        /// <param name="productId">This is product id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ProductInsuranceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("product/{productId}")]
        public async Task<ActionResult> CalculateInsurance([FromRoute] int productId)
        {
            var insurance = await _insuranceService.CalculateProductInsurance(productId);

            return Ok(insurance);
        }

        /// <summary>
        /// Calculate the insurance cost for a shopping cart
        /// </summary>
        /// <param name="cartInsuranceRequest">Shopping Cart request body</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(CartInsuranceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("product")]
        public async Task<ActionResult> CalculateCartInsurance(CartInsuranceRequest cartInsuranceRequest)
        {
            var cartInsurance = await _insuranceService.CalculateCartInsurance(cartInsuranceRequest);

            return Ok(cartInsurance);
        }
    }
}
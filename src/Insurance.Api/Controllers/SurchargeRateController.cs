using Insurance.Api.Models.Request;
using Insurance.Api.Services.Surcharge;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Insurance.Api.Controllers
{
    [ApiController]
    [Route("api/surcharge-rates")]
    public class SurchargeRateController : Controller
    {
        private readonly ISurchargeRateService _surchargeRateService;

        public SurchargeRateController(ISurchargeRateService surchargeRateService)
        {
            _surchargeRateService = surchargeRateService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllSurchargeRates()
        {
            var surchargeRates = await _surchargeRateService.GetAllSurchargeRates();

            return Ok(surchargeRates);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetSurchargeRateById(int id)
        {
            var surchargeRates = await _surchargeRateService.GetSurchargeRateById(id);

            return Ok(surchargeRates);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSurchargeRate([FromBody] CreateSurchargeRateRequest request)
        {
            var surchargeRate = await _surchargeRateService.CreateSurchargeRate(request);

            return Ok(surchargeRate);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSurchargeRateById(int id, [FromBody] UpdateSurchargeRateRequest request)
        {
            var surchargeRate = await _surchargeRateService.UpdateSurchargeRateById(id, request);

            return Ok(surchargeRate);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteSurchargeRateById(int id)
        {
            await _surchargeRateService.DeleteSurchargeRateById(id);

            return NoContent();
        }
    }
}

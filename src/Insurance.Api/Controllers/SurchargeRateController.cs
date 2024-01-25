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
        public async Task<ActionResult> GetAll()
        {
            var surchargeRates = await _surchargeRateService.GetAll();

            return Ok(surchargeRates);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var surchargeRates = await _surchargeRateService.GetById(id);

            return Ok(surchargeRates);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateSurchargeRateRequest request)
        {
            var surchargeRate = await _surchargeRateService.Create(request);

            return Ok(surchargeRate);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateById(int id, [FromBody] UpdateSurchargeRateRequest request)
        {
            var surchargeRate = await _surchargeRateService.UpdateById(id, request);

            return Ok(surchargeRate);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteById(int id)
        {
            await _surchargeRateService.DeleteById(id);

            return NoContent();
        }
    }
}

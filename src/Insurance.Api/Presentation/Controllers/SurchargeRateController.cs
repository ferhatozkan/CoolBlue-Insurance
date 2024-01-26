using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.Application.Models.Dto;
using Insurance.Api.Application.Services.Surcharge;
using Insurance.Api.Presentation.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Presentation.Controllers
{
    /// <summary>
    /// This api handles all the logic for surcharge rate.
    /// </summary>  
    [ApiController]
    [Route("api/surcharge-rates")]
    public class SurchargeRateController : Controller
    {
        private readonly ISurchargeRateService _surchargeRateService;

        public SurchargeRateController(ISurchargeRateService surchargeRateService)
        {
            _surchargeRateService = surchargeRateService;
        }

        /// <summary>
        /// Get all surcharge rates
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<SurchargeRateDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAll()
        {
            var surchargeRates = await _surchargeRateService.GetAll();

            return Ok(surchargeRates);
        }

        /// <summary>
        /// Get a surcharge rate by id
        /// </summary>
        /// <param name="id">This is surcharge rate id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SurchargeRateDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetById(int id)
        {
            var surchargeRate = await _surchargeRateService.GetById(id);

            return Ok(surchargeRate);
        }

        /// <summary>
        /// Create a surcharge rate
        /// </summary>
        /// <param name="request">Create Surcharge Rate request body</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(SurchargeRateDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create([FromBody] CreateSurchargeRateRequest request)
        {
            var surchargeRate = await _surchargeRateService.Create(request);

            return Ok(surchargeRate);
        }

        /// <summary>
        /// Update an existing surcharge rate
        /// </summary>
        /// <param name="id">This is surcharge rate id</param>
        /// <param name="request">Update Surcharge Rate request body</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SurchargeRateDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateById(int id, [FromBody] UpdateSurchargeRateRequest request)
        {
            var surchargeRate = await _surchargeRateService.UpdateById(id, request);

            return Ok(surchargeRate);
        }

        /// <summary>
        /// Delete an existing surcharge rate
        /// </summary>
        /// <param name="id">This is surcharge rate id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteById(int id)
        {
            await _surchargeRateService.DeleteById(id);

            return NoContent();
        }
    }
}

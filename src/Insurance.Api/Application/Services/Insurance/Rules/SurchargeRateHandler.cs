using Insurance.Api.Application.Models.Dto;
using Insurance.Api.Application.Repositories;
using Insurance.Api.Application.Services.Insurance.Chain;
using Microsoft.Extensions.Logging;

namespace Insurance.Api.Application.Services.Insurance.Rules
{
    public class SurchargeRateHandler : AbstractHandler
    {
        private readonly ISurchargeRateRepository _surchargeRateRepository;
        private readonly ILogger<SurchargeRateHandler> _logger;

        public SurchargeRateHandler(ISurchargeRateRepository surchargeRateRepository, ILogger<SurchargeRateHandler> logger)
        {
            _surchargeRateRepository = surchargeRateRepository;
            _logger = logger;
        }

        public override InsuranceDto Handle(InsuranceDto insuranceDto)
        {
            var surcharge = _surchargeRateRepository.GetByProductTypeIdAsync(insuranceDto.ProductTypeId).Result;
            if (surcharge != null)
            {
                var surchargeCost = insuranceDto.SalesPrice * ((double)surcharge.Rate / 100);

                insuranceDto.InsuranceCost += surchargeCost;

                _logger.LogInformation($"Surcharge cost was calculated {surchargeCost} for product {insuranceDto.ProductId} and productTypeId {insuranceDto.ProductTypeId}");
            }


            return NextChain(insuranceDto);
        }
    }
}

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

        public override ProductInsuranceChainDto Handle(ProductInsuranceChainDto productInsuranceDto)
        {
            var surcharge = _surchargeRateRepository.GetByProductTypeIdAsync(productInsuranceDto.ProductTypeId).Result;
            if (surcharge != null)
            {
                var surchargeCost = productInsuranceDto.SalesPrice * ((double)surcharge.Rate / 100);

                productInsuranceDto.InsuranceCost += surchargeCost;

                _logger.LogInformation($"Surcharge cost was calculated {surchargeCost} for product {productInsuranceDto.ProductId} and productTypeId {productInsuranceDto.ProductTypeId}");
            }


            return NextChain(productInsuranceDto);
        }
    }
}

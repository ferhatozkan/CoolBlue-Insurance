using Insurance.Api.Models.Dto;
using Insurance.Api.Repository;
using Insurance.Api.Services.Insurance.Models;
using Microsoft.Extensions.Logging;
using System;

namespace Insurance.Api.Services.Chain
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

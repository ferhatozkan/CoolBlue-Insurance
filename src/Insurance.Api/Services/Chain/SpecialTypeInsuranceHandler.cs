using Insurance.Api.Constants;
using Insurance.Api.Models.Dto;
using Insurance.Api.Services.Insurance.Models;
using Microsoft.Extensions.Logging;
using System;

namespace Insurance.Api.Services.Chain
{
    public class SpecialTypeInsuranceHandler : AbstractHandler
    {
        private readonly ILogger<SpecialTypeInsuranceHandler> _logger;

        public SpecialTypeInsuranceHandler(ILogger<SpecialTypeInsuranceHandler> logger)
        {
            _logger = logger;
        }

        public override ProductInsuranceChainDto Handle(ProductInsuranceChainDto productInsuranceDto)
        {
            if (Enum.IsDefined(typeof(SpecialProductType), productInsuranceDto.ProductTypeId))
            {
                productInsuranceDto.InsuranceCost += InsuranceRuleConstants.SpecialTypeInsuranceCost;

                _logger.LogInformation($"Special product type rule insurance cost was calculated {InsuranceRuleConstants.SpecialTypeInsuranceCost} for product {productInsuranceDto.ProductId} and productTypeId {productInsuranceDto.ProductTypeId}");
            }

            return NextChain(productInsuranceDto);
        }
    }
}

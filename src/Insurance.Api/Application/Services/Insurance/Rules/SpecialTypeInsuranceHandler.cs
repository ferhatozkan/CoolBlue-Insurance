using System;
using Insurance.Api.Application.Models.Dto;
using Insurance.Api.Application.Services.Insurance.Chain;
using Insurance.Api.Domain.Constants;
using Insurance.Api.Domain.Models.Enums;
using Microsoft.Extensions.Logging;

namespace Insurance.Api.Application.Services.Insurance.Rules
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

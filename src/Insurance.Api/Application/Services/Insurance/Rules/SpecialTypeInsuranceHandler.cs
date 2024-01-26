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

        public override InsuranceDto Handle(InsuranceDto insuranceDto)
        {
            if (Enum.IsDefined(typeof(SpecialProductType), insuranceDto.ProductTypeId))
            {
                insuranceDto.InsuranceCost += InsuranceRuleConstants.SpecialTypeInsuranceCost;

                _logger.LogInformation($"Special product type rule insurance cost was calculated {InsuranceRuleConstants.SpecialTypeInsuranceCost} for product {insuranceDto.ProductId} and productTypeId {insuranceDto.ProductTypeId}");
            }

            return NextChain(insuranceDto);
        }
    }
}

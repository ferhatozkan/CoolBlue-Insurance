using System.Linq;
using Insurance.Api.Application.Models.Dto;
using Insurance.Api.Application.Services.Insurance.Chain;
using Insurance.Api.Domain.Constants;
using Insurance.Api.Domain.Models.Entities;
using Microsoft.Extensions.Logging;

namespace Insurance.Api.Application.Services.Insurance.Rules
{
    public class PriceRuleInsuranceHandler : AbstractHandler
    {
        private readonly ILogger<PriceRuleInsuranceHandler> _logger;

        public PriceRuleInsuranceHandler(ILogger<PriceRuleInsuranceHandler> logger)
        {
            _logger = logger;
        }

        public override ProductInsuranceChainDto Handle(ProductInsuranceChainDto productInsuranceDto)
        {
            var productInsuranceRule = InsuranceRuleConstants.ProductInsuranceRules
                .First(rule => FindInsuranceRule(rule, productInsuranceDto.SalesPrice));

            productInsuranceDto.InsuranceCost += productInsuranceRule.InsurancePrice;

            _logger.LogInformation($"Sales price range rule insurance cost was calculated {productInsuranceRule.InsurancePrice} for product {productInsuranceDto.ProductId}");

            return NextChain(productInsuranceDto);
        }

        private bool FindInsuranceRule(ProductInsuranceRule rule, double salesPrice)
        {
            if (rule.MaxSalesPrice == null && rule.MinSalesPrice == null)
                return false;

            if ((rule.MaxSalesPrice == null || salesPrice < rule.MaxSalesPrice)
                && (rule.MinSalesPrice == null || salesPrice >= rule.MinSalesPrice))
            {
                return true;
            }

            return false;
        }
    }
}

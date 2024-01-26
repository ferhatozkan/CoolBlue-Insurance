using Insurance.Api.Constants;
using Insurance.Api.Models.Dto;
using Insurance.Api.Services.Insurance.Models;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Insurance.Api.Services.Chain
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

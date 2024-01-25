using Insurance.Api.Clients;
using Insurance.Api.Constants;
using Insurance.Api.Models;
using Insurance.Api.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insurance.Api.Services
{
    public class InsuranceService : IInsuranceService
    {
        private readonly IProductApiClient _productApiClient;

        public InsuranceService(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        public async Task<InsuranceDto> CalculateInsurance(int productId)
        {
            var product = await _productApiClient.GetProduct(productId);
            var productType = await _productApiClient.GetProductType(product.ProductTypeId);

            if (!productType.CanBeInsured)
            {
                return new InsuranceDto
                {
                    ProductId = productId,
                    InsuranceCost = 0
                };
            }

            double insuranceValue = 0;
            foreach (var rule in InsuranceRuleConstants.Ranges)
            {
                insuranceValue += CalculateInsuranceRule(rule, product.SalesPrice);
            };

            if (Enum.IsDefined(typeof(SpecialProductType), productType.Name))
                insuranceValue += 500;

            var insurance = new InsuranceDto()
            {
                ProductId = productId,
                InsuranceCost = insuranceValue
            };

            return insurance;
        }

        public Task<CartInsuranceDto> CalculateCartInsurance(List<int> productIds)
        {
            throw new NotImplementedException();
        }

        private double CalculateInsuranceRule(InsuranceRule rule, double salesPrice)
        {
            if (rule.MaxSalesPrice == null && rule.MinSalesPrice == null)
                return 0;

            if ((rule.MaxSalesPrice == null || salesPrice < rule.MaxSalesPrice)
                && (rule.MinSalesPrice == null || salesPrice >= rule.MinSalesPrice))
            {
                return rule.InsurancePrice;
            }

            return 0;
        }
    }
}

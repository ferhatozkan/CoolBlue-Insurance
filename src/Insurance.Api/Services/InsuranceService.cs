using Insurance.Api.Clients;
using Insurance.Api.Constants;
using Insurance.Api.Models;
using Insurance.Api.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<InsuranceDto> CalculateProductInsurance(int productId)
        {
            return await CalculateInsurance(productId);
        }

        public async Task<CartInsuranceDto> CalculateCartInsurance(List<int> productIds)
        {
            var cartInsurance = new CartInsuranceDto();
            foreach (var productId in productIds)
            {
                var insurace = await CalculateInsurance(productId);
                cartInsurance.Products.Add(insurace);
            }

            var productsInsurance = cartInsurance.Products.Sum(i => i.InsuranceCost);

            var cartProductTypes = cartInsurance.Products.Select(p => p.ProductTypeId).Distinct().ToList();
            var frequenlyLostProductsInsurance = ApplyCartInsurance(cartProductTypes);

            cartInsurance.TotalInsuranceCost = productsInsurance + frequenlyLostProductsInsurance;

            return cartInsurance;
        }

        private double ApplyCartInsurance(List<int> cartProductTypes)
        {
            double insuranceCost = 0;
            foreach (var cartProductType in cartProductTypes)
            {
                var cartInsuranceCost = InsuranceRuleConstants.CartInsuranceRules.GetValueOrDefault((FrequentlyLostProductType) cartProductType);
                insuranceCost += cartInsuranceCost;
            }
            return insuranceCost;
        }

        private async Task<InsuranceDto> CalculateInsurance(int productId)
        {
            var product = await _productApiClient.GetProduct(productId);
            var productType = await _productApiClient.GetProductType(product.ProductTypeId);

            if (!productType.CanBeInsured)
            {
                return new InsuranceDto
                {
                    ProductId = productId,
                    InsuranceCost = 0,
                    ProductTypeId = productType.Id
                };
            }

            double insuranceValue = 0;
            foreach (var rule in InsuranceRuleConstants.ProductInsuranceRules)
            {
                insuranceValue += CalculateInsuranceRule(rule, product.SalesPrice);
            };

            if (Enum.IsDefined(typeof(SpecialProductType), productType.Id))
                insuranceValue += 500;

            var insurance = new InsuranceDto()
            {
                ProductId = productId,
                InsuranceCost = insuranceValue,
                ProductTypeId = productType.Id
            };

            return insurance;
        }

        private double CalculateInsuranceRule(ProductInsuranceRule rule, double salesPrice)
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

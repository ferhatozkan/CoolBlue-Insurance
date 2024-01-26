using Insurance.Api.Clients;
using Insurance.Api.Constants;
using Insurance.Api.Models.Dto;
using Insurance.Api.Models.Request;
using Insurance.Api.Repository;
using Insurance.Api.Services.Insurance.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insurance.Api.Services.Insurance
{
    public class InsuranceService : IInsuranceService
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ISurchargeRateRepository _surchargeRateRepository;
        private readonly ILogger<InsuranceService> _logger;

        public InsuranceService(
            IProductApiClient productApiClient,
            ISurchargeRateRepository surchargeRateRepository,
            ILogger<InsuranceService> logger)
        {
            _productApiClient = productApiClient;
            _surchargeRateRepository = surchargeRateRepository;
            _logger = logger;
        }

        public async Task<InsuranceDto> CalculateProductInsurance(int productId)
        {
            _logger.LogInformation($"CalculateProductInsurance was invoked with productId {productId} parameter on {DateTime.UtcNow}");

            return await CalculateInsurance(productId);
        }

        public async Task<CartInsuranceDto> CalculateCartInsurance(CartRequest cartRequest)
        {
            var productIds = cartRequest.CartItems.Select(ci => ci.ProductId);
            _logger.LogInformation($"CalculateCartInsurance was invoked with productIds {string.Join(",", productIds)} parameter on {DateTime.UtcNow}");

            var insuranceDtos = new List<InsuranceDto>();
            foreach (var item in cartRequest.CartItems)
            {
                var insurace = await CalculateInsurance(item.ProductId);
                insuranceDtos.Add(insurace);
            }

            var sumInsurance = insuranceDtos.Sum(p => p.InsuranceCost);

            _logger.LogInformation($"Products insurance cost was calculated {sumInsurance} for cart");

            var cartProductTypes = insuranceDtos.Select(p => p.ProductTypeId).Distinct().ToList();
            var applyCartInsurance = ApplyCartInsurance(cartProductTypes);

            _logger.LogInformation($"Frequenly Lost Product Insurance cost was calculated {applyCartInsurance} for cart");

            var totalInsuranceCost = sumInsurance + applyCartInsurance;

            _logger.LogInformation($"Total Insurance Cost was calculated {totalInsuranceCost} for cart");

            var cartInsuraceItems = insuranceDtos.Select(insurance => new CartInsuranceItemDto
            {
                ProductId = insurance.ProductId,
                InsuranceCost = insurance.InsuranceCost
            }).ToList();

            var cartInsurance = new CartInsuranceDto
            {
                TotalInsuranceCost = totalInsuranceCost,
                CartInsuranceItems = cartInsuraceItems
            };

            return cartInsurance;
        }

        private double ApplyCartInsurance(List<int> cartProductTypes)
        {
            double insuranceCost = 0;
            foreach (var cartProductType in cartProductTypes)
            {
                var cartInsuranceCost = InsuranceRuleConstants.CartInsuranceRules.GetValueOrDefault((FrequentlyLostProductType)cartProductType);
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

            _logger.LogInformation($"Sales price range rule insurance cost was calculated {insuranceValue} for product {productId}");

            if (Enum.IsDefined(typeof(SpecialProductType), productType.Id)) 
            {
                var specialProductInsuranceCost = 500;

                insuranceValue += specialProductInsuranceCost;

                _logger.LogInformation($"Special product type rule insurance cost was calculated {specialProductInsuranceCost} for product {productId} and productTypeId {productType.Id}");
            }

            var surcharge = await _surchargeRateRepository.GetByProductTypeIdAsync(productType.Id);
            if (surcharge != null)
            {
                var surchargeCost = product.SalesPrice * ((double)surcharge.Rate / 100);

                insuranceValue += surchargeCost;

                _logger.LogInformation($"Surcharge cost was calculated {surchargeCost} for product {productId} and productTypeId {productType.Id}");
            }

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

using Insurance.Api.Clients;
using Insurance.Api.Constants;
using Insurance.Api.Models.Dto;
using Insurance.Api.Models.Request;
using Insurance.Api.Repository;
using Insurance.Api.Services.Chain;
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
        private readonly ILogger<InsuranceService> _logger;
        private readonly IInsuranceChainService _insuranceChainService;

        public InsuranceService(
            IProductApiClient productApiClient,
            ILogger<InsuranceService> logger,
            IInsuranceChainService insuranceChainService)
        {
            _productApiClient = productApiClient;
            _logger = logger;
            _insuranceChainService = insuranceChainService;
        }

        public async Task<ProductInsuranceDto> CalculateProductInsurance(int productId)
        {
            _logger.LogInformation($"CalculateProductInsurance was invoked with productId {productId} on {DateTime.UtcNow}");

            var productInsurance = await CalculateInsurance(productId);

            var productInsuranceDto = new ProductInsuranceDto
            {
                ProductId = productInsurance.ProductId,
                InsuranceCost = productInsurance.InsuranceCost
            };

            return productInsuranceDto;
        }

        public async Task<CartInsuranceDto> CalculateCartInsurance(CartRequest cartRequest)
        {
            var productIds = cartRequest.CartItems.Select(ci => ci.ProductId);
            _logger.LogInformation($"CalculateCartInsurance was invoked with productIds {string.Join(",", productIds)} on {DateTime.UtcNow}");

            var itemInsuranceDtos = new List<InsuranceDto>();
            foreach (var item in cartRequest.CartItems)
            {
                var itemInsuranceDto = await CalculateInsurance(item.ProductId);
                itemInsuranceDtos.Add(itemInsuranceDto);
            }

            var productInsuranceSum = itemInsuranceDtos.Sum(p => p.InsuranceCost);

            _logger.LogInformation($"Products insurance cost was calculated {productInsuranceSum} Euros");

            var cartProductTypes = itemInsuranceDtos.Select(p => p.ProductTypeId).Distinct().ToList();
            var cartInsurance = ApplyCartInsurance(cartProductTypes);

            _logger.LogInformation($"Cart insurance cost was calculated {cartInsurance} Euros");

            var totalInsuranceCost = productInsuranceSum + cartInsurance;

            _logger.LogInformation($"Total cart insurance cost was calculated {totalInsuranceCost} Euros");

            var cartInsuranceDto = new CartInsuranceDto
            {
                TotalInsuranceCost = totalInsuranceCost,
                CartInsuranceItems = itemInsuranceDtos.Select(insurance => new CartInsuranceItemDto
                {
                    ProductId = insurance.ProductId,
                    InsuranceCost = insurance.InsuranceCost
                }).ToList()
            };

            return cartInsuranceDto;
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

            var productInsuranceDto = _insuranceChainService.Handle(new ProductInsuranceChainDto
            {
                InsuranceCost = 0,
                ProductId = product.Id,
                ProductTypeId = product.ProductTypeId,
                SalesPrice = product.SalesPrice
            });

            var insurance = new InsuranceDto()
            {
                ProductId = productId,
                InsuranceCost = productInsuranceDto.InsuranceCost,
                ProductTypeId = productInsuranceDto.ProductTypeId
            };

            return insurance;
        }
    }
}

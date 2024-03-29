﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Insurance.Api.Application.Clients;
using Insurance.Api.Application.Models.Dto;
using Insurance.Api.Application.Services.Insurance.Chain;
using Insurance.Api.Domain.Constants;
using Insurance.Api.Domain.Models.Enums;
using Insurance.Api.Presentation.Models.Requests;
using Microsoft.Extensions.Logging;

namespace Insurance.Api.Application.Services.Insurance
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

        public async Task<CartInsuranceDto> CalculateCartInsurance(CartInsuranceRequest cartInsuranceRequest)
        {
            var productIds = cartInsuranceRequest.CartItems.Select(ci => ci.ProductId);
            _logger.LogInformation($"CalculateCartInsurance was invoked with productIds {string.Join(",", productIds)} on {DateTime.UtcNow}");

            var insuranceDtos = new List<InsuranceDto>();
            foreach (var item in cartInsuranceRequest.CartItems)
            {
                var insuranceDto = await CalculateInsurance(item.ProductId);
                insuranceDtos.Add(insuranceDto);
            }

            var productInsuranceSum = insuranceDtos.Sum(p => p.InsuranceCost);

            _logger.LogInformation($"Products insurance cost was calculated {productInsuranceSum} Euros");

            var cartProductTypes = insuranceDtos.Select(p => p.ProductTypeId).Distinct().ToList();
            var cartInsurance = ApplyCartInsurance(cartProductTypes);

            _logger.LogInformation($"Cart insurance cost was calculated {cartInsurance} Euros");

            var totalInsuranceCost = productInsuranceSum + cartInsurance;

            _logger.LogInformation($"Total cart insurance cost was calculated {totalInsuranceCost} Euros");

            var cartInsuranceDto = new CartInsuranceDto
            {
                TotalInsuranceCost = totalInsuranceCost,
                CartInsuranceItems = insuranceDtos.Select(insurance => new CartInsuranceItemDto
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

            var productInsuranceDto = _insuranceChainService.Handle(new InsuranceDto
            {
                InsuranceCost = 0,
                ProductId = product.Id,
                ProductTypeId = product.ProductTypeId,
                SalesPrice = product.SalesPrice
            });

            var insurance = new InsuranceDto
            {
                ProductId = productId,
                InsuranceCost = productInsuranceDto.InsuranceCost,
                ProductTypeId = productInsuranceDto.ProductTypeId
            };

            return insurance;
        }
    }
}

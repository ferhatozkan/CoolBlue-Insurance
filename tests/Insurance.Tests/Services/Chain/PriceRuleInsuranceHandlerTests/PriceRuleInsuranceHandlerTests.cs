﻿using Insurance.Api.Models.Dto;
using Insurance.Api.Services.Chain;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Insurance.Tests.Services.Chain.PriceRuleInsuranceHandlerTests
{
    public class PriceRuleInsuranceHandlerTests
    {
        private readonly PriceRuleInsuranceHandler _priceRuleInsuranceHandler;

        public PriceRuleInsuranceHandlerTests()
        {
            _priceRuleInsuranceHandler = new PriceRuleInsuranceHandler(Mock.Of<ILogger<PriceRuleInsuranceHandler>>());
        }

        [Theory, ClassData(typeof(PriceRuleInsuranceHandlerData))]
        public void GivenCanBeInsuredTrue_ShouldNotAddToInsuranceCost(ProductInsuranceChainTestDto chainDto)
        {
            var productInsuranceChainDto = new ProductInsuranceChainDto
            {
                ProductId = chainDto.ProductId,
                ProductTypeId = chainDto.ProductTypeId,
                SalesPrice = chainDto.SalesPrice,
                InsuranceCost = chainDto.InsuranceCost
            };
            var result = _priceRuleInsuranceHandler.Handle(productInsuranceChainDto);
            Assert.NotNull(result);
            Assert.Equal(chainDto.ExpectedInsuranceCost, result.InsuranceCost);
        }
    }
}
﻿using Insurance.Api.Models.Dto;
using Insurance.Api.Models.Entities;
using Insurance.Api.Repository;
using Insurance.Api.Services.Chain;
using Insurance.Tests.Services.Chain.PriceRuleInsuranceHandlerTests;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Services.Chain.SurchargeRateHandlerTests
{
    public class SurchargeRateHandlerTests
    {
        private readonly SurchargeRateHandler _surchargeRateHandler;
        private readonly Mock<ISurchargeRateRepository> _surchargeRateRepository;

        public SurchargeRateHandlerTests()
        {
            _surchargeRateRepository = new Mock<ISurchargeRateRepository>();
            _surchargeRateHandler = new SurchargeRateHandler(_surchargeRateRepository.Object, Mock.Of<ILogger<SurchargeRateHandler>>());
        }

        [Theory, ClassData(typeof(SurchargeRateHandlerData))]
        public void GivenProducts_ShouldCheckProductTypeAndAddInsuranceCost(ProductInsuranceChainTestDto chainDto)
        {
            var productInsuranceChainDto = new ProductInsuranceChainDto
            {
                ProductId = chainDto.ProductId,
                ProductTypeId = chainDto.ProductTypeId,
                SalesPrice = chainDto.SalesPrice,
                InsuranceCost = chainDto.InsuranceCost
            };

            _surchargeRateRepository.Setup(repository => repository.GetByProductTypeIdAsync(chainDto.ProductTypeId))
                .Returns(Task.FromResult(new SurchargeRate
                {
                    Rate = chainDto.Rate
                }));

            var result = _surchargeRateHandler.Handle(productInsuranceChainDto);
            Assert.NotNull(result);
            Assert.Equal(chainDto.ExpectedInsuranceCost, result.InsuranceCost);
        }
    }
}
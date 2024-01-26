using Insurance.Api.Models.Dto;
using Insurance.Api.Services.Chain;
using Insurance.Tests.Services.Chain.PriceRuleInsuranceHandlerTests;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Insurance.Tests.Services.Chain.SpecialTypeInsuranceHandlerTests
{
    public class SpecialTypeInsuranceHandlerTests
    {
        private readonly SpecialTypeInsuranceHandler _specialTypeInsuranceHandler;

        public SpecialTypeInsuranceHandlerTests()
        {
            _specialTypeInsuranceHandler = new SpecialTypeInsuranceHandler(Mock.Of<ILogger<SpecialTypeInsuranceHandler>>());
        }

        [Theory, ClassData(typeof(SpecialTypeInsuranceHandlerData))]
        public void GivenProducts_ShouldCheckProductTypeAndAddInsuranceCost(ProductInsuranceChainTestDto chainDto)
        {
            var productInsuranceChainDto = new ProductInsuranceChainDto
            {
                ProductId = chainDto.ProductId,
                ProductTypeId = chainDto.ProductTypeId,
                SalesPrice = chainDto.SalesPrice,
                InsuranceCost = chainDto.InsuranceCost
            };
            var result = _specialTypeInsuranceHandler.Handle(productInsuranceChainDto);
            Assert.NotNull(result);
            Assert.Equal(chainDto.ExpectedInsuranceCost, result.InsuranceCost);
        }
    }
}

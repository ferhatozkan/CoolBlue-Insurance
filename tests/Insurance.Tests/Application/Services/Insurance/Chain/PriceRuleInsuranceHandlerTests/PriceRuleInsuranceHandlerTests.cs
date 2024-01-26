using Insurance.Api.Application.Models.Dto;
using Insurance.Api.Application.Services.Insurance.Rules;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Insurance.Tests.Application.Services.Insurance.Chain.PriceRuleInsuranceHandlerTests
{
    public class PriceRuleInsuranceHandlerTests
    {
        private readonly PriceRuleInsuranceHandler _priceRuleInsuranceHandler;

        public PriceRuleInsuranceHandlerTests()
        {
            _priceRuleInsuranceHandler = new PriceRuleInsuranceHandler(Mock.Of<ILogger<PriceRuleInsuranceHandler>>());
        }

        [Theory, ClassData(typeof(PriceRuleInsuranceHandlerData))]
        public void GivenCanBeInsuredTrue_ShouldNotAddToInsuranceCost(double expected, InsuranceDto chainDto)
        {
            var result = _priceRuleInsuranceHandler.Handle(chainDto);
            Assert.NotNull(result);
            Assert.Equal(expected, result.InsuranceCost);
        }
    }
}

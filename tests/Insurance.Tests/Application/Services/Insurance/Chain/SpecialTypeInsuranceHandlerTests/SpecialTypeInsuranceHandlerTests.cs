using Insurance.Api.Application.Models.Dto;
using Insurance.Api.Application.Services.Insurance.Rules;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Insurance.Tests.Application.Services.Insurance.Chain.SpecialTypeInsuranceHandlerTests
{
    public class SpecialTypeInsuranceHandlerTests
    {
        private readonly SpecialTypeInsuranceHandler _specialTypeInsuranceHandler;

        public SpecialTypeInsuranceHandlerTests()
        {
            _specialTypeInsuranceHandler = new SpecialTypeInsuranceHandler(Mock.Of<ILogger<SpecialTypeInsuranceHandler>>());
        }

        [Theory, ClassData(typeof(SpecialTypeInsuranceHandlerData))]
        public void GivenProducts_ShouldCheckProductTypeAndAddInsuranceCost(double expected, InsuranceDto chainDto)
        {
            var result = _specialTypeInsuranceHandler.Handle(chainDto);
            Assert.NotNull(result);
            Assert.Equal(expected, result.InsuranceCost);
        }
    }
}

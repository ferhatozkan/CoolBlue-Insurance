using System.Threading.Tasks;
using Insurance.Api.Application.Models.Dto;
using Insurance.Api.Application.Repositories;
using Insurance.Api.Application.Services.Insurance.Rules;
using Insurance.Api.Domain.Models.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Insurance.Tests.Application.Services.Insurance.Chain.SurchargeRateHandlerTests
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
        public void GivenProducts_ShouldCheckProductTypeAndAddInsuranceCost(int rate, double expected, InsuranceDto chainDto)
        {
            _surchargeRateRepository.Setup(repository => repository.GetByProductTypeIdAsync(chainDto.ProductTypeId))
                .Returns(Task.FromResult(new SurchargeRate
                {
                    Rate = rate
                }));

            var result = _surchargeRateHandler.Handle(chainDto);
            Assert.NotNull(result);
            Assert.Equal(expected, result.InsuranceCost);
        }
    }
}

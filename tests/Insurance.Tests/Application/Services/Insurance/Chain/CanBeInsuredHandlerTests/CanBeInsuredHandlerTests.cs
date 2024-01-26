using System.Threading.Tasks;
using Insurance.Api.Application.Clients;
using Insurance.Api.Application.Models.Dto;
using Insurance.Api.Application.Services.Insurance.Rules;
using Insurance.Api.Infrastructure.Clients.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Insurance.Tests.Application.Services.Insurance.Chain.CanBeInsuredHandlerTests
{
    public class CanBeInsuredHandlerTests
    {
        private readonly Mock<IProductApiClient> _productApiClient;
        private readonly CanBeInsuredHandler _canBeInsuredHandler;

        public CanBeInsuredHandlerTests()
        {
            _productApiClient = new Mock<IProductApiClient>();
            _canBeInsuredHandler = new CanBeInsuredHandler(_productApiClient.Object, Mock.Of<ILogger<CanBeInsuredHandler>>());
        }

        [Fact]
        public void GivenCanBeInsuredTrue_ShouldMoveToNextChain()
        {
            _productApiClient.Setup(client => client.GetProductType(It.IsAny<int>()))
                .Returns(Task.FromResult(new ProductType
                {
                    CanBeInsured = true
                }));

            var result = _canBeInsuredHandler.Handle(new ProductInsuranceChainDto());
            Assert.NotNull(result);
            Assert.Equal(0, result.InsuranceCost);
        }

        [Fact]
        public void GivenCanBeInsuredFalse_ShouldReturnReturn0EuroInsuranceCost()
        {
            _productApiClient.Setup(client => client.GetProductType(It.IsAny<int>()))
                .Returns(Task.FromResult(new ProductType
                {
                    CanBeInsured = false
                }));

            var result = _canBeInsuredHandler.Handle(new ProductInsuranceChainDto());
            Assert.NotNull(result);
            Assert.Equal(0, result.InsuranceCost);
        }
    }
}

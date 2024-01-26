using Insurance.Api.Clients;
using Insurance.Api.Clients.Models;
using Insurance.Api.Models.Dto;
using Insurance.Api.Services.Chain;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Services.Chain.CanBeInsuredHandlerTests
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
                .Returns(Task.FromResult(new ProductTypeDto
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
                .Returns(Task.FromResult(new ProductTypeDto
                {
                    CanBeInsured = false
                }));

            var result = _canBeInsuredHandler.Handle(new ProductInsuranceChainDto());
            Assert.NotNull(result);
            Assert.Equal(0, result.InsuranceCost);
        }
    }
}

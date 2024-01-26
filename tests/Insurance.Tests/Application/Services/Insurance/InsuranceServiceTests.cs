using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.Application.Clients;
using Insurance.Api.Application.Models.Dto;
using Insurance.Api.Application.Services.Insurance;
using Insurance.Api.Application.Services.Insurance.Chain;
using Insurance.Api.Infrastructure.Clients.Models;
using Insurance.Api.Presentation.Models.Requests;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Insurance.Tests.Application.Services.Insurance
{
    public class InsuranceServiceTests
    {
        private Mock<IProductApiClient> _productApiClient;
        private Mock<IInsuranceChainService> _insuranceChainService;

        private InsuranceService _insuranceService;

        public InsuranceServiceTests()
        {
            _productApiClient = new Mock<IProductApiClient>();
            _insuranceChainService = new Mock<IInsuranceChainService>();

            _insuranceService = new InsuranceService(_productApiClient.Object, Mock.Of<ILogger<InsuranceService>>(),
                _insuranceChainService.Object);
        }

        [Fact]
        public async Task GivenGetProductThrowsException_CalculateProductInsuranceShouldThrowException()
        {
            _productApiClient.Setup(client => client.GetProduct(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _insuranceService.CalculateProductInsurance(1));
        }

        [Fact]
        public async Task GivenGetProductThrowsException_CalculateCartInsuranceShouldThrowException()
        {
            _productApiClient.Setup(client => client.GetProduct(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            var cartRequest = new CartInsuranceRequest
            {
                CartItems = new List<CartInsuranceItem>
                {
                    new CartInsuranceItem
                    {
                        ProductId = 1
                    }
                }
            };

            await Assert.ThrowsAsync<Exception>(async () =>
                await _insuranceService.CalculateCartInsurance(cartRequest));
        }

        [Theory, ClassData(typeof(CalculateCartInsuranceData))]
        public async Task GivenCartProducts_CalculateCartInsuranceShouldReturnExpectedCartInsuranceCost(double expected,
            List<Product> products)
        {
            var totalInsuranceCost = expected;
            var cartRequest = new CartInsuranceRequest
            {
                CartItems = new List<CartInsuranceItem>()
            };

            foreach (var product in products)
            {
                var expectedInsuranceChainDto = new InsuranceDto
                {
                    ProductId = product.Id,
                    ProductTypeId = product.ProductTypeId,
                    SalesPrice = product.SalesPrice,
                    InsuranceCost = 0
                };

                cartRequest.CartItems.Add(new CartInsuranceItem
                {
                    ProductId = product.Id
                });

                _productApiClient.Setup(client => client.GetProduct(product.Id))
                    .Returns(Task.FromResult(product));

                _insuranceChainService.Setup(service => service.Handle(It.IsAny<InsuranceDto>()))
                    .Returns(expectedInsuranceChainDto);
            }

            var result = await _insuranceService.CalculateCartInsurance(cartRequest);

            Assert.NotNull(result);
            Assert.Equal(expected: totalInsuranceCost, result.TotalInsuranceCost);
        }
    }
}
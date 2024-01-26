using Insurance.Api.Clients;
using Insurance.Api.Clients.Models;
using Insurance.Api.Models.Dto;
using Insurance.Api.Models.Entities;
using Insurance.Api.Models.Request;
using Insurance.Api.Services.Chain;
using Insurance.Api.Services.Insurance;
using Insurance.Tests.Services.Chain.PriceRuleInsuranceHandlerTests;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Services
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

            _insuranceService = new InsuranceService(_productApiClient.Object, Mock.Of<ILogger<InsuranceService>>(), _insuranceChainService.Object);
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

            var cartRequest = new CartRequest
            {
                CartItems = new List<CartItem>
                {
                    new CartItem
                    {
                        ProductId = 1
                    }
                }
            };

            await Assert.ThrowsAsync<Exception>(async () => await _insuranceService.CalculateCartInsurance(cartRequest));
        }

        [Theory, ClassData(typeof(CalculateCartInsuranceData))]
        public async Task GivenCartProducts_CalculateCartInsuranceShouldReturnExpectedCartInsuranceCost(CartRequestTestDto data)
        {
            var totalInsuranceCost = data.TotalInsuranceCost;
            var cartRequest = new CartRequest 
            {
                CartItems = new List<CartItem>()
            };

            foreach (var item in data.CartItems)
            {
                var productInsuranceChainDto = new ProductInsuranceChainDto
                {
                    ProductId = item.ProductId,
                    ProductTypeId = item.ProductTypeId,
                    SalesPrice = item.SalesPrice,
                    InsuranceCost = 0
                };
                
                var expectedInsuranceChainDto = new ProductInsuranceChainDto
                {
                    ProductId = item.ProductId,
                    ProductTypeId = item.ProductTypeId,
                    SalesPrice = item.SalesPrice,
                    InsuranceCost = 0
                };

                var product = new ProductDto
                {
                    Id = item.ProductId,
                    ProductTypeId = item.ProductTypeId,
                    SalesPrice = item.SalesPrice
                };

                cartRequest.CartItems.Add(new CartItem
                {
                    ProductId = item.ProductId
                });

                _productApiClient.Setup(client => client.GetProduct(item.ProductId))
                        .Returns(Task.FromResult(product));

                _insuranceChainService.Setup(service => service.Handle(It.IsAny<ProductInsuranceChainDto>()))
                        .Returns(expectedInsuranceChainDto);
            }

            var result = await _insuranceService.CalculateCartInsurance(cartRequest);

            Assert.NotNull(result);
            Assert.Equal(expected: totalInsuranceCost, result.TotalInsuranceCost);
        }
    }
}

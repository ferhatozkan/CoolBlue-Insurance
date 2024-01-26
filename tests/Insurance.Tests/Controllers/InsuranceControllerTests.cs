using Insurance.Api.Controllers;
using Moq;
using System.Threading.Tasks;
using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Insurance.Api.Services.Insurance;
using Insurance.Api.Models.Dto;
using Insurance.Api.Models.Request;

namespace Insurance.Tests.Controllers
{
    public class InsuranceControllerTests
    {

        private Mock<IInsuranceService> _insuranceService;
        private InsuranceController _insuranceController;

        public InsuranceControllerTests()
        {
            _insuranceService = new Mock<IInsuranceService>();
            _insuranceController = new InsuranceController(_insuranceService.Object);
        }

        [Fact]
        public async Task GivenCalculateInsuranceSuccessfully_ShouldReturn200StatusCode()
        {
            var insurance = new ProductInsuranceDto
            {
                ProductId = 1,
                InsuranceCost = 100
            };

            _insuranceService.Setup(service => service.CalculateProductInsurance(It.IsAny<int>()))
                .Returns(Task.FromResult(insurance));

            var result = await _insuranceController.CalculateInsurance(1);

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ProductInsuranceDto>(okObjectResult.Value);

            Assert.Equal(1, response.ProductId);
            Assert.Equal(100, response.InsuranceCost);
        }

        [Fact]
        public async Task GivenCalculateInsuranceThrowsException_ShouldThrowException()
        {
            _insuranceService.Setup(service => service.CalculateProductInsurance(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _insuranceController.CalculateInsurance(1));
        }

        [Fact]
        public async Task GivenCalculateCartInsuranceSuccessfully_ShouldReturn200StatusCode()
        {
            var cartInsurance = new CartInsuranceDto
            {
                TotalInsuranceCost = 3000,
                CartInsuranceItems = new List<CartInsuranceItemDto> 
                {
                    new CartInsuranceItemDto { ProductId = 1, InsuranceCost = 500 },
                    new CartInsuranceItemDto { ProductId = 1, InsuranceCost = 2500}
                }
            };

            _insuranceService.Setup(service => service.CalculateCartInsurance(It.IsAny<CartRequest>()))
                .Returns(Task.FromResult(cartInsurance));

            var result = await _insuranceController.CalculateCartInsurance(new CartRequest());

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CartInsuranceDto>(okObjectResult.Value);

            Assert.Equal(3000, response.TotalInsuranceCost);
            Assert.Equal(cartInsurance.CartInsuranceItems, response.CartInsuranceItems);
        }
    }
}

using Insurance.Api.Controllers;
using Insurance.Api.Services;
using Moq;
using System.Threading.Tasks;
using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Insurance.Api.Models;

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
            var insurance = new InsuranceDto
            {
                ProductId = 1,
                InsuranceValue = 100
            };

            _insuranceService.Setup(client => client.CalculateInsurance(It.IsAny<int>()))
                .Returns(Task.FromResult(insurance));

            var result = await _insuranceController.CalculateInsurance(1);

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<InsuranceDto>(okObjectResult.Value);

            Assert.Equal(1, response.ProductId);
            Assert.Equal(100, response.InsuranceValue);
        }

        [Fact]
        public async Task GivenCalculateInsuranceThrowsException_ShouldThrowException()
        {
            _insuranceService.Setup(client => client.CalculateInsurance(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _insuranceController.CalculateInsurance(1));
        }
    }
}

using Moq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Insurance.Api.Services.Surcharge;
using Insurance.Api.Controllers;
using Insurance.Api.Models.Dto;
using System.Collections.Generic;
using System;
using Insurance.Api.Models.Request;

namespace Insurance.Tests.Controllers
{
    public class SurchargeRateControllerTest
    {

        private Mock<ISurchargeRateService> _surchargeRateService;
        private SurchargeRateController _surchargeRateController;

        public SurchargeRateControllerTest()
        {
            _surchargeRateService = new Mock<ISurchargeRateService>();
            _surchargeRateController = new SurchargeRateController(_surchargeRateService.Object);
        }

        [Fact]
        public async Task GivenGetAllSurchargeRatesSuccessfully_ShouldReturn200StatusCode()
        {
            _surchargeRateService.Setup(client => client.GetAllSurchargeRates())
                .Returns(Task.FromResult(new List<SurchargeRateDto>()));

            var result = await _surchargeRateController.GetAllSurchargeRates();

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<List<SurchargeRateDto>>(okObjectResult.Value);
        }

        [Fact]
        public async Task GivenGetAllSurchargeRatesThrowsException_ShouldThrowException()
        {
            _surchargeRateService.Setup(client => client.GetAllSurchargeRates())
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _surchargeRateController.GetAllSurchargeRates());
        }

        [Fact]
        public async Task GivenGetSurchargeRateByIdSuccessfully_ShouldReturn200StatusCode()
        {
            _surchargeRateService.Setup(client => client.GetSurchargeRateById(It.IsAny<int>()))
                .Returns(Task.FromResult(new SurchargeRateDto()));

            var result = await _surchargeRateController.GetSurchargeRateById(1);

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<SurchargeRateDto>(okObjectResult.Value);
        }

        [Fact]
        public async Task GivenGetSurchargeRateByIdThrowsException_ShouldThrowExceptio()
        {
            _surchargeRateService.Setup(client => client.GetSurchargeRateById(1))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _surchargeRateController.GetSurchargeRateById(1));
        }

        [Fact]
        public async Task GivenCreateSurchargeRateSuccessfully_ShouldReturn200StatusCode()
        {
            _surchargeRateService.Setup(client => client.CreateSurchargeRate(It.IsAny<CreateSurchargeRateRequest>()))
                .Returns(Task.FromResult(new SurchargeRateDto()));

            var result = await _surchargeRateController.CreateSurchargeRate(new CreateSurchargeRateRequest());

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<SurchargeRateDto>(okObjectResult.Value);
        }

        [Fact]
        public async Task GivenCreateSurchargeRateThrowsException_ShouldThrowException()
        {
            _surchargeRateService.Setup(client => client.GetAllSurchargeRates())
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _surchargeRateController.GetAllSurchargeRates());
        }


        [Fact]
        public async Task GivenUpdateSurchargeRateByIdSuccessfully_ShouldReturn200StatusCode()
        {
            _surchargeRateService.Setup(client => client.UpdateSurchargeRateById(It.IsAny<int>(), It.IsAny<UpdateSurchargeRateRequest>()))
                .Returns(Task.FromResult(new SurchargeRateDto()));

            var result = await _surchargeRateController.UpdateSurchargeRateById(1, new UpdateSurchargeRateRequest());

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<SurchargeRateDto>(okObjectResult.Value);
        }

        [Fact]
        public async Task GivenUpdateSurchargeRateByIdThrowsException_ShouldThrowException()
        {
            _surchargeRateService.Setup(client => client.UpdateSurchargeRateById(It.IsAny<int>(), It.IsAny<UpdateSurchargeRateRequest>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _surchargeRateController.UpdateSurchargeRateById(1, new UpdateSurchargeRateRequest()));
        }

        [Fact]
        public async Task GivenDeleteSurchargeRateByIdSuccessfully_ShouldReturn200StatusCode()
        {
            _surchargeRateService.Setup(client => client.DeleteSurchargeRateById(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var result = await _surchargeRateController.DeleteSurchargeRateById(1);

            var okObjectResult = Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GivenDeleteSurchargeRateByIdThrowsException_ShouldThrowException()
        {
            _surchargeRateService.Setup(client => client.DeleteSurchargeRateById(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _surchargeRateController.DeleteSurchargeRateById(1));
        }
    }
}

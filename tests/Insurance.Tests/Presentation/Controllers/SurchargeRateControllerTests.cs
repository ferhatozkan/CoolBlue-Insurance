﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.Application.Models.Dto;
using Insurance.Api.Application.Services.Surcharge;
using Insurance.Api.Presentation.Controllers;
using Insurance.Api.Presentation.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Insurance.Tests.Presentation.Controllers
{
    public class SurchargeRateControllerTests
    {

        private Mock<ISurchargeRateService> _surchargeRateService;
        private SurchargeRateController _surchargeRateController;

        public SurchargeRateControllerTests()
        {
            _surchargeRateService = new Mock<ISurchargeRateService>();
            _surchargeRateController = new SurchargeRateController(_surchargeRateService.Object);
        }

        [Fact]
        public async Task GivenGetAllSuccessfully_ShouldReturn200StatusCode()
        {
            _surchargeRateService.Setup(service => service.GetAll())
                .Returns(Task.FromResult(new List<SurchargeRateDto>()));

            var result = await _surchargeRateController.GetAll();

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<List<SurchargeRateDto>>(okObjectResult.Value);
        }

        [Fact]
        public async Task GivenGetAllThrowsException_ShouldThrowException()
        {
            _surchargeRateService.Setup(service => service.GetAll())
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _surchargeRateController.GetAll());
        }

        [Fact]
        public async Task GivenGetByIdSuccessfully_ShouldReturn200StatusCode()
        {
            _surchargeRateService.Setup(service => service.GetById(It.IsAny<int>()))
                .Returns(Task.FromResult(new SurchargeRateDto()));

            var result = await _surchargeRateController.GetById(1);

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<SurchargeRateDto>(okObjectResult.Value);
        }

        [Fact]
        public async Task GivenGetByIdThrowsException_ShouldThrowExceptio()
        {
            _surchargeRateService.Setup(service => service.GetById(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _surchargeRateController.GetById(1));
        }

        [Fact]
        public async Task GivenCreateSuccessfully_ShouldReturn200StatusCode()
        {
            _surchargeRateService.Setup(service => service.Create(It.IsAny<CreateSurchargeRateRequest>()))
                .Returns(Task.FromResult(new SurchargeRateDto()));

            var result = await _surchargeRateController.Create(new CreateSurchargeRateRequest());

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<SurchargeRateDto>(okObjectResult.Value);
        }

        [Fact]
        public async Task GivenCreateThrowsException_ShouldThrowException()
        {
            _surchargeRateService.Setup(service => service.Create(It.IsAny<CreateSurchargeRateRequest>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _surchargeRateController.Create(new CreateSurchargeRateRequest()));
        }


        [Fact]
        public async Task GivenUpdateByIdSuccessfully_ShouldReturn200StatusCode()
        {
            _surchargeRateService.Setup(service => service.UpdateById(It.IsAny<int>(), It.IsAny<UpdateSurchargeRateRequest>()))
                .Returns(Task.FromResult(new SurchargeRateDto()));

            var result = await _surchargeRateController.UpdateById(1, new UpdateSurchargeRateRequest());

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<SurchargeRateDto>(okObjectResult.Value);
        }

        [Fact]
        public async Task GivenUpdateByIdThrowsException_ShouldThrowException()
        {
            _surchargeRateService.Setup(service => service.UpdateById(It.IsAny<int>(), It.IsAny<UpdateSurchargeRateRequest>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _surchargeRateController.UpdateById(1, new UpdateSurchargeRateRequest()));
        }

        [Fact]
        public async Task GivenDeleteByIdSuccessfully_ShouldReturn200StatusCode()
        {
            _surchargeRateService.Setup(service => service.DeleteById(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var result = await _surchargeRateController.DeleteById(1);

            var okObjectResult = Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GivenDeleteByIdThrowsException_ShouldThrowException()
        {
            _surchargeRateService.Setup(service => service.DeleteById(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _surchargeRateController.DeleteById(1));
        }
    }
}

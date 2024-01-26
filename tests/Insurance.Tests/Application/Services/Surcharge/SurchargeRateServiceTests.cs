using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.Application.Exceptions;
using Insurance.Api.Application.Repositories;
using Insurance.Api.Application.Services.Surcharge;
using Insurance.Api.Domain.Models.Entities;
using Insurance.Api.Presentation.Models.Requests;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Insurance.Tests.Application.Services.Surcharge
{
    public class SurchargeRateServiceTests
    {
        private Mock<ISurchargeRateRepository> _surchargeRateRepository;
        private SurchargeRateService _surchargeRateService;

        public SurchargeRateServiceTests()
        {
            _surchargeRateRepository = new Mock<ISurchargeRateRepository>();
            _surchargeRateService = new SurchargeRateService(_surchargeRateRepository.Object, Mock.Of<ILogger<SurchargeRateService>>());
        }

        [Fact]
        public async Task GivenGetAllAsyncSuccess_GetAllShouldReturnSurchargeRates()
        {
            _surchargeRateRepository.Setup(repository => repository.GetAllAsync())
                .Returns(Task.FromResult(new List<SurchargeRate>()));

            var surchargeRates = await _surchargeRateService.GetAll();
            Assert.NotNull(surchargeRates);
        }

        [Fact]
        public async Task GivenGetAllAsyncThrowsException_GetAllShouldThrowException()
        {
            _surchargeRateRepository.Setup(repository => repository.GetAllAsync())
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _surchargeRateService.GetAll());
        }

        [Fact]
        public async Task GivenGetByIdAsyncReturnsNotNull_GetByIdShouldReturnSurchargeRate()
        {
            _surchargeRateRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new SurchargeRate()));

            var surchargeRate = await _surchargeRateService.GetById(1);
            Assert.NotNull(surchargeRate);
        }

        [Fact]
        public async Task GivenGetByIdAsyncReturnsNull_GetByIdShouldThrowNotFoundException()
        {
            _surchargeRateRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((SurchargeRate)null));

            await Assert.ThrowsAsync<NotFoundException>(async () => await _surchargeRateService.GetById(1));
        }

        [Fact]
        public async Task GivenGetByProductTypeIdAsyncReturnsNullAndCreateAsyncIsSuccess_CreateShouldReturnSurchargeRate()
        {
            _surchargeRateRepository.Setup(repository => repository.GetByProductTypeIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((SurchargeRate)null));

            _surchargeRateRepository.Setup(repository => repository.CreateAsync(It.IsAny<SurchargeRate>()))
                .Returns(Task.CompletedTask);

            var surchargeRate = await _surchargeRateService.Create(new CreateSurchargeRateRequest());
            Assert.NotNull(surchargeRate);
        }

        [Fact]
        public async Task GivenGetByProductTypeIdAsyncReturnsNotNull_CreateShouldThrowNotFoundException()
        {
            _surchargeRateRepository.Setup(repository => repository.GetByProductTypeIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new SurchargeRate()));

            await Assert.ThrowsAsync<BadRequestException>(async () => await _surchargeRateService.Create(new CreateSurchargeRateRequest()));
        }

        [Fact]
        public async Task GivenGetByIdAsyncReturnsNotNull_DeleteByIdShouldDeleteSuccessfully()
        {
            _surchargeRateRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new SurchargeRate()));

            _surchargeRateRepository.Setup(repository => repository.DeleteByIdAsync(It.IsAny<SurchargeRate>()))
                .Returns(Task.CompletedTask);

            await _surchargeRateService.DeleteById(1);

            _surchargeRateRepository.Verify(repository => repository.DeleteByIdAsync(It.IsAny<SurchargeRate>()));
        }

        [Fact]
        public async Task GivenGetByIdAsyncReturnsNull_DeleteByIdShouldThrowNotFoundException()
        {
            _surchargeRateRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((SurchargeRate)null));

            await Assert.ThrowsAsync<NotFoundException>(async () => await _surchargeRateService.DeleteById(1));
        }

        [Fact]
        public async Task GiveGetByIdAsyncReturnsNull_UpdateByIdShouldThrowNotFoundException()
        {
            _surchargeRateRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((SurchargeRate)null));

            await Assert.ThrowsAsync<NotFoundException>(async () => await _surchargeRateService.UpdateById(1, new UpdateSurchargeRateRequest()));
        }

        [Fact]
        public async Task GivenGetByIdAsyncReturnsNotNullAndGetByProductTypeIdAsyncReturnsNotNull_UpdateByIdShouldThrowBadRequestException()
        {
            _surchargeRateRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new SurchargeRate()
                {
                    Id = 1,
                }));

            _surchargeRateRepository.Setup(repository => repository.GetByProductTypeIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new SurchargeRate()
                {
                    Id = 4
                }));

            await Assert.ThrowsAsync<BadRequestException>(async () => await _surchargeRateService.UpdateById(1, new UpdateSurchargeRateRequest()));
        }

        [Fact]
        public async Task GivenGetByIdAsyncReturnsNull_UpdateByIdShouldThrowNotFoundException()
        {
            _surchargeRateRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<int>()))
               .Returns(Task.FromResult(new SurchargeRate()));

            _surchargeRateRepository.Setup(repository => repository.GetByProductTypeIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((SurchargeRate)null));

            var surchargeRate = await _surchargeRateService.UpdateById(1, new UpdateSurchargeRateRequest());
            Assert.NotNull(surchargeRate);
        }
    }
}

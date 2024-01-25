using Insurance.Api.Repository;
using Insurance.Api.Services.Surcharge;
using Moq;
using System.Threading.Tasks;
using System;
using Xunit;
using Insurance.Api.Models.Entities;
using System.Collections.Generic;
using Insurance.Api.Models.Request;

namespace Insurance.Tests.Services
{
    public class SurchargeRateServiceTests
    {
        private Mock<ISurchargeRateRepository> _surchargeRateRepository;
        private SurchargeRateService _surchargeRateService;

        public SurchargeRateServiceTests()
        {
            _surchargeRateRepository = new Mock<ISurchargeRateRepository>();
            _surchargeRateService = new SurchargeRateService(_surchargeRateRepository.Object);
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
        public async Task GivenGetByIdAsyncSuccess_GetByIdShouldReturnSurchargeRate()
        {
            _surchargeRateRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new SurchargeRate()));

            var surchargeRate = await _surchargeRateService.GetById(1);
            Assert.NotNull(surchargeRate);
        }

        [Fact]
        public async Task GivenGetByIdAsyncThrowsException_GetByIdShouldThrowException()
        {
            _surchargeRateRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _surchargeRateService.GetById(1));
        }
        
        [Fact]
        public async Task GivenCreateAsyncSuccess_CreateShouldReturnSurchargeRate()
        {
            _surchargeRateRepository.Setup(repository => repository.CreateAsync(It.IsAny<SurchargeRate>()))
                .Returns(Task.CompletedTask);

            var surchargeRate = await _surchargeRateService.Create(new CreateSurchargeRateRequest());
            Assert.NotNull(surchargeRate);
        }

        [Fact]
        public async Task GivenCreateAsyncThrowsException_CreateShouldThrowException()
        {
            _surchargeRateRepository.Setup(repository => repository.CreateAsync(It.IsAny<SurchargeRate>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _surchargeRateService.Create(new CreateSurchargeRateRequest()));
        }

        [Fact]
        public async Task GivenDeleteByIdAsyncSuccess_DeleteByIdShouldReturnSurchargeRate()
        {
            _surchargeRateRepository.Setup(repository => repository.DeleteByIdAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            await _surchargeRateService.DeleteById(1);

            _surchargeRateRepository.Verify(repository => repository.DeleteByIdAsync(It.IsAny<int>()));
        }

        [Fact]
        public async Task GivenDeleteByIdAsyncThrowsException_DeleteByIdThrowException()
        {
            _surchargeRateRepository.Setup(repository => repository.DeleteByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _surchargeRateService.DeleteById(1));
        }

        [Fact]
        public async Task GivenSaveAsyncAndGetByIdAsyncSuccess_UpdateShouldReturnSurchargeRate()
        {
            _surchargeRateRepository.Setup(repository => repository.SaveAsync())
                .Returns(Task.CompletedTask);

            _surchargeRateRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<int>()))
              .Returns(Task.FromResult(new SurchargeRate
              {
                  Id = 1
              }));

            var surchargeRate = await _surchargeRateService.UpdateById(1, new UpdateSurchargeRateRequest());
            Assert.NotNull(surchargeRate);
        }

        [Fact]
        public async Task GivenGetByIdAsyncThrowsException_UpdateByIdShouldThrowException()
        {
            _surchargeRateRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _surchargeRateService.UpdateById(1, new UpdateSurchargeRateRequest()));
        }
    }
}

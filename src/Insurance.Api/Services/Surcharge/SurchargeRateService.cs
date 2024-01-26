using Insurance.Api.Exceptions;
using Insurance.Api.Models.Dto;
using Insurance.Api.Models.Entities;
using Insurance.Api.Models.Request;
using Insurance.Api.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Insurance.Api.Services.Surcharge
{
    public class SurchargeRateService : ISurchargeRateService
    {
        private readonly ISurchargeRateRepository _surchargeRateRepository;

        private readonly ILogger<SurchargeRateService> _logger;

        public SurchargeRateService(ISurchargeRateRepository surchargeRateRepository, ILogger<SurchargeRateService> logger)
        {
            _surchargeRateRepository = surchargeRateRepository;
            _logger = logger;
        }
        public async Task<List<SurchargeRateDto>> GetAll()
        {
            _logger.LogInformation($"GetAll was invoked on {DateTime.UtcNow}");

            var surchargeRates = await _surchargeRateRepository.GetAllAsync();

            return surchargeRates.Select(sr => new SurchargeRateDto
            {
                Id = sr.Id,
                Name = sr.Name,
                Rate = sr.Rate,
                ProductTypeId = sr.ProductTypeId,
            }).ToList();
        }

        public async Task<SurchargeRateDto> GetById(int id)
        {
            _logger.LogInformation($"GetById was invoked with Id {id} parameter on {DateTime.UtcNow}");

            var surchargeRate = await _surchargeRateRepository.GetByIdAsync(id);
            if(surchargeRate == null)
                throw new NotFoundException($"SurchargeRate with id {id} not found");

            return new SurchargeRateDto
            {
                Id = surchargeRate.Id,
                Name = surchargeRate.Name,
                Rate = surchargeRate.Rate,
                ProductTypeId = surchargeRate.ProductTypeId,
            };
        }

        public async Task<SurchargeRateDto> Create(CreateSurchargeRateRequest request)
        {
            _logger.LogInformation($"Create was invoked with CreateSurchargeRateRequest {JsonSerializer.Serialize(request)} parameter on {DateTime.UtcNow}");

            var surchargeRate = await _surchargeRateRepository.GetByProductTypeIdAsync(request.ProductTypeId);
            if (surchargeRate != null)
                throw new BadRequestException($"SurchargeRate with productType id {request.ProductTypeId} already exists");

            var newSurchargeRate = new SurchargeRate
            {
                Name = request.Name,
                Rate = request.Rate,
                ProductTypeId = request.ProductTypeId,
            };

            await _surchargeRateRepository.CreateAsync(newSurchargeRate);

            return new SurchargeRateDto
            {
                Id = newSurchargeRate.Id,
                Name = newSurchargeRate.Name,
                Rate = newSurchargeRate.Rate,
                ProductTypeId = newSurchargeRate.ProductTypeId
            };
        }

        public async Task DeleteById(int id)
        {
            _logger.LogInformation($"DeleteById was invoked with Id {id} parameter on {DateTime.UtcNow}");

            var surchargeRule = await _surchargeRateRepository.GetByIdAsync(id);
            if (surchargeRule == null)
                throw new NotFoundException($"Surcharge rate with Id {id} cannot be found");

            await _surchargeRateRepository.DeleteByIdAsync(surchargeRule);
        }

        public async Task<SurchargeRateDto> UpdateById(int id, UpdateSurchargeRateRequest request)
        {
            _logger.LogInformation($"UpdateById was invoked with Id {id} and UpdateSurchargeRateRequest {JsonSerializer.Serialize(request)} parameters on {DateTime.UtcNow}");

            var surchargeRate = await _surchargeRateRepository.GetByIdAsync(id);

            if (surchargeRate == null)
                throw new NotFoundException($"Surcharge rate with Id {id} cannot be found");

            var surchargeRateByType = await _surchargeRateRepository.GetByProductTypeIdAsync(request.ProductTypeId);
            if (surchargeRateByType != null && surchargeRateByType.Id != surchargeRate.Id)
                throw new BadRequestException($"Surcharge rate with productType id {request.ProductTypeId} already exists");

            surchargeRate.Name = request.Name;
            surchargeRate.Rate = request.Rate;
            surchargeRate.ProductTypeId = request.ProductTypeId;

            await _surchargeRateRepository.SaveAsync();
            return new SurchargeRateDto
            {
                Id = surchargeRate.Id,
                Name = surchargeRate.Name,
                Rate = surchargeRate.Rate,
                ProductTypeId = surchargeRate.ProductTypeId
            };
        }
    }
}

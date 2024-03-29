﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Insurance.Api.Application.Exceptions;
using Insurance.Api.Application.Models.Dto;
using Insurance.Api.Application.Repositories;
using Insurance.Api.Domain.Models.Entities;
using Insurance.Api.Presentation.Models.Requests;
using Microsoft.Extensions.Logging;

namespace Insurance.Api.Application.Services.Surcharge
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
            _logger.LogInformation($"GetById was invoked with id {id} on {DateTime.UtcNow}");

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
            _logger.LogInformation($"Create was invoked with CreateSurchargeRateRequest {JsonSerializer.Serialize(request)} on {DateTime.UtcNow}");

            var surchargeRate = await _surchargeRateRepository.GetByProductTypeIdAsync(request.ProductTypeId);
            if (surchargeRate != null)
                throw new BadRequestException($"SurchargeRate with ProductTypeId {request.ProductTypeId} already exists");

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
            _logger.LogInformation($"DeleteById was invoked with id {id} on {DateTime.UtcNow}");

            var surchargeRule = await _surchargeRateRepository.GetByIdAsync(id);
            if (surchargeRule == null)
                throw new NotFoundException($"Surcharge rate with Id {id} cannot be found");

            await _surchargeRateRepository.DeleteByIdAsync(surchargeRule);
        }

        public async Task<SurchargeRateDto> UpdateById(int id, UpdateSurchargeRateRequest request)
        {
            _logger.LogInformation($"UpdateById was invoked with id {id} and UpdateSurchargeRateRequest {JsonSerializer.Serialize(request)} on {DateTime.UtcNow}");

            var surchargeRate = await _surchargeRateRepository.GetByIdAsync(id);

            if (surchargeRate == null)
                throw new NotFoundException($"Surcharge rate with Id {id} cannot be found");

            var surchargeRateByType = await _surchargeRateRepository.GetByProductTypeIdAsync(request.ProductTypeId);
            if (surchargeRateByType != null && surchargeRateByType.Id != surchargeRate.Id)
                throw new BadRequestException($"Surcharge rate with ProductTypeId {request.ProductTypeId} already exists");

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

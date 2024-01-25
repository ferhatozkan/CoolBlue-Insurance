using Insurance.Api.Models.Dto;
using Insurance.Api.Models.Entities;
using Insurance.Api.Models.Request;
using Insurance.Api.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insurance.Api.Services.Surcharge
{
    public class SurchargeRateService : ISurchargeRateService
    {
        private readonly ISurchargeRateRepository _surchargeRateRepository;

        public SurchargeRateService(ISurchargeRateRepository surchargeRateRepository)
        {
            _surchargeRateRepository = surchargeRateRepository;
        }
        public async Task<List<SurchargeRateDto>> GetAll()
        {
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
            var surchargeRate = await _surchargeRateRepository.GetByIdAsync(id);
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
            var surchargeRate = new SurchargeRate
            {
                Name = request.Name,
                Rate = request.Rate,
                ProductTypeId = request.ProductTypeId,
            };

            await _surchargeRateRepository.CreateAsync(surchargeRate);

            return new SurchargeRateDto
            {
                Id = surchargeRate.Id,
                Name = surchargeRate.Name,
                Rate = surchargeRate.Rate,
                ProductTypeId = surchargeRate.ProductTypeId
            };
        }

        public async Task DeleteById(int id)
        {
            await _surchargeRateRepository.DeleteByIdAsync(id);
        }

        public async Task<SurchargeRateDto> UpdateById(int id, UpdateSurchargeRateRequest request)
        {
            var surchargeRate = await _surchargeRateRepository.GetByIdAsync(id);

            if (surchargeRate == null)
                throw new Exception();

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

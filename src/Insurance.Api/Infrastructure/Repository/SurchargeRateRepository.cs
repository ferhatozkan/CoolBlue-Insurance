﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.Application.Repositories;
using Insurance.Api.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Api.Infrastructure.Repository
{
    public class SurchargeRateRepository : ISurchargeRateRepository
    {
        private readonly DataContext _dataContext;
        public SurchargeRateRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<SurchargeRate>> GetAllAsync()
        {
            return await _dataContext.SurchargeRates.ToListAsync();
        }

        public async Task<SurchargeRate> GetByIdAsync(int id)
        {
            return await _dataContext.SurchargeRates.FindAsync(id);
        }

        public async Task<SurchargeRate> GetByProductTypeIdAsync(int productTypeId)
        {
            return await _dataContext.SurchargeRates.FirstOrDefaultAsync(sr => sr.ProductTypeId == productTypeId);
        }

        public async Task CreateAsync(SurchargeRate surchargeRate)
        {
            await _dataContext.SurchargeRates.AddAsync(surchargeRate);
            await _dataContext.SaveChangesAsync();
        }

        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(SurchargeRate surchargeRate)
        {
            _dataContext.SurchargeRates.Remove(surchargeRate);
            await _dataContext.SaveChangesAsync();
        }
    }
}

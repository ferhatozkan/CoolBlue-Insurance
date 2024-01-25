using Insurance.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insurance.Api.Repository
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

        public async Task CreateAsync(SurchargeRate surchargeRate)
        {
            await _dataContext.SurchargeRates.AddAsync(surchargeRate);
            await _dataContext.SaveChangesAsync();
        }

        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            _dataContext.SurchargeRates.Remove(new SurchargeRate() { Id = id });
            await _dataContext.SaveChangesAsync();
        }
    }
}

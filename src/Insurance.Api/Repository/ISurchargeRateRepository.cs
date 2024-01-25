using Insurance.Api.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insurance.Api.Repository
{
    public interface ISurchargeRateRepository
    {
        Task<List<SurchargeRate>> GetAllAsync();
        Task<SurchargeRate> GetByIdAsync(int id);
        Task CreateAsync(SurchargeRate surchargeRate);
        Task SaveAsync();
        Task DeleteByIdAsync(int id);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.Domain.Models.Entities;

namespace Insurance.Api.Application.Repositories
{
    public interface ISurchargeRateRepository
    {
        Task<List<SurchargeRate>> GetAllAsync();
        Task<SurchargeRate> GetByIdAsync(int id);
        Task<SurchargeRate> GetByProductTypeIdAsync(int productTypeId);
        Task CreateAsync(SurchargeRate surchargeRate);
        Task SaveAsync();
        Task DeleteByIdAsync(SurchargeRate surchargeRate);
    }
}

using Insurance.Api.Models.Dto;
using Insurance.Api.Models.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insurance.Api.Services.Surcharge
{
    public interface ISurchargeRateService
    {
        Task<List<SurchargeRateDto>> GetAllSurchargeRates();
        Task<SurchargeRateDto> GetSurchargeRateById(int id);
        Task DeleteSurchargeRateById(int id);
        Task<SurchargeRateDto> CreateSurchargeRate(CreateSurchargeRateRequest request);
        Task<SurchargeRateDto> UpdateSurchargeRateById(int id, UpdateSurchargeRateRequest request);
    }
}

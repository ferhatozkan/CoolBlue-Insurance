using Insurance.Api.Models.Dto;
using Insurance.Api.Models.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insurance.Api.Services.Surcharge
{
    public interface ISurchargeRateService
    {
        Task<List<SurchargeRateDto>> GetAll();
        Task<SurchargeRateDto> GetById(int id);
        Task DeleteById(int id);
        Task<SurchargeRateDto> Create(CreateSurchargeRateRequest request);
        Task<SurchargeRateDto> UpdateById(int id, UpdateSurchargeRateRequest request);
    }
}

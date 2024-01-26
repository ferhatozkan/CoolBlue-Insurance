using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.Application.Models.Dto;
using Insurance.Api.Presentation.Models.Requests;

namespace Insurance.Api.Application.Services.Surcharge
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

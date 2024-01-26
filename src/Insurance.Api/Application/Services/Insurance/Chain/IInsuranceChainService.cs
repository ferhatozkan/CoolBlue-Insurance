using Insurance.Api.Application.Models.Dto;

namespace Insurance.Api.Application.Services.Insurance.Chain
{
    public interface IInsuranceChainService
    {
        InsuranceDto Handle(InsuranceDto insuranceDto);
    }
}

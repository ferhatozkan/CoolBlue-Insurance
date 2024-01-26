using Insurance.Api.Application.Models.Dto;

namespace Insurance.Api.Application.Services.Insurance.Chain
{
    public interface IHandler
    {
        InsuranceDto Handle(InsuranceDto insuranceDto);
        IHandler SetNext(IHandler handler);
    }
}

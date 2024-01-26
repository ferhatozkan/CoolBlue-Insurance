using Insurance.Api.Application.Models.Dto;

namespace Insurance.Api.Application.Services.Insurance.Chain
{
    public interface IHandler
    {
        ProductInsuranceChainDto Handle(ProductInsuranceChainDto insuranceDto);
        IHandler SetNext(IHandler handler);
    }
}

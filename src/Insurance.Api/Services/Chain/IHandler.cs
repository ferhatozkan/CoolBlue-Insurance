using Insurance.Api.Models.Dto;

namespace Insurance.Api.Services.Chain
{
    public interface IHandler
    {
        ProductInsuranceChainDto Handle(ProductInsuranceChainDto insuranceDto);
        IHandler SetNext(IHandler handler);
    }
}

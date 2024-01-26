using Insurance.Api.Application.Models.Dto;

namespace Insurance.Api.Application.Services.Insurance.Chain
{
    public interface IInsuranceChainService
    {
        ProductInsuranceChainDto Handle(ProductInsuranceChainDto productInsuranceDto);
    }
}

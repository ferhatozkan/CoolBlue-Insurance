using Insurance.Api.Models.Dto;

namespace Insurance.Api.Services.Chain
{
    public interface IInsuranceChainService
    {
        ProductInsuranceChainDto Handle(ProductInsuranceChainDto productInsuranceDto);
    }
}

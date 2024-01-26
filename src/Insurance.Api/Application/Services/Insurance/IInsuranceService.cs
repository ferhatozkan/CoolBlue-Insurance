using System.Threading.Tasks;
using Insurance.Api.Application.Models.Dto;
using Insurance.Api.Presentation.Models.Requests;

namespace Insurance.Api.Application.Services.Insurance
{
    public interface IInsuranceService
    {
        Task<ProductInsuranceDto> CalculateProductInsurance(int productId);
        Task<CartInsuranceDto> CalculateCartInsurance(CartInsuranceRequest cartInsuranceRequest);
    }
}

using Insurance.Api.Models.Dto;
using Insurance.Api.Models.Request;
using System.Threading.Tasks;

namespace Insurance.Api.Services.Insurance
{
    public interface IInsuranceService
    {
        Task<ProductInsuranceDto> CalculateProductInsurance(int productId);
        Task<CartInsuranceDto> CalculateCartInsurance(CartRequest cartRequest);
    }
}

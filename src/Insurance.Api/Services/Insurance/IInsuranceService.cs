using Insurance.Api.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insurance.Api.Services.Insurance
{
    public interface IInsuranceService
    {
        Task<InsuranceDto> CalculateProductInsurance(int productId);
        Task<CartInsuranceDto> CalculateCartInsurance(List<int> productIds);
    }
}

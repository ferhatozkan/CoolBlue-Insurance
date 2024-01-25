using Insurance.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insurance.Api.Services
{
    public interface IInsuranceService
    {
        Task<InsuranceDto> CalculateProductInsurance(int productId);
        Task<CartInsuranceDto> CalculateCartInsurance(List<int> productIds);
    }
}

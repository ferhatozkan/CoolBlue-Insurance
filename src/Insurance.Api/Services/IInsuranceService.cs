using Insurance.Api.Models;
using System.Threading.Tasks;

namespace Insurance.Api.Services
{
    public interface IInsuranceService
    {
        Task<InsuranceDto> CalculateInsurance(int productId);
    }
}

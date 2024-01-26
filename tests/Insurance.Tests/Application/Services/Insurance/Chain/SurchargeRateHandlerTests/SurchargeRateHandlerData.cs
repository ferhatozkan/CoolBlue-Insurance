using Insurance.Api.Application.Models.Dto;
using Xunit;

namespace Insurance.Tests.Application.Services.Insurance.Chain.SurchargeRateHandlerTests
{
    public class SurchargeRateHandlerData : TheoryData<int, double, InsuranceDto>
    {
        public SurchargeRateHandlerData()
        {
            Add(
                10,
                100,
                new InsuranceDto
                {
                    ProductId = 1,
                    ProductTypeId = 1,
                    SalesPrice = 1000,
                    InsuranceCost = 0
                }
            );
            Add(
                50,
                1000,
                new InsuranceDto
                {
                    ProductId = 1,
                    ProductTypeId = 2,
                    SalesPrice = 2000,
                    InsuranceCost = 0
                }
            );
            Add(
                20,
                100,
                new InsuranceDto
                {
                    ProductId = 1,
                    ProductTypeId = 3,
                    SalesPrice = 500,
                    InsuranceCost = 0
                }
            );
        }
    }
}

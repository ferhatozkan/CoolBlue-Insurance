using Insurance.Api.Application.Models.Dto;
using Xunit;

namespace Insurance.Tests.Application.Services.Insurance.Chain.SpecialTypeInsuranceHandlerTests
{
    public class SpecialTypeInsuranceHandlerData : TheoryData<double, InsuranceDto>
    {
        public SpecialTypeInsuranceHandlerData()
        {
            Add(
                500,
                new InsuranceDto
                {
                    ProductId = 1,
                    ProductTypeId = 32,
                    SalesPrice = 100,
                    InsuranceCost = 0
                }
            );
            Add(
                500,
                new InsuranceDto
                {
                    ProductId = 1,
                    ProductTypeId = 21,
                    SalesPrice = 500,
                    InsuranceCost = 0
                }
            );
            Add(
                0,
                new InsuranceDto
                {
                    ProductId = 1,
                    ProductTypeId = 2,
                    SalesPrice = 600,
                    InsuranceCost = 0
                }
            );
        }
    }
}

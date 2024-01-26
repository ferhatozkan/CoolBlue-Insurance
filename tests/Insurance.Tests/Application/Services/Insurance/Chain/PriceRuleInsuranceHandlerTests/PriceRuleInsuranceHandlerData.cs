using Insurance.Api.Application.Models.Dto;
using Xunit;

namespace Insurance.Tests.Application.Services.Insurance.Chain.PriceRuleInsuranceHandlerTests
{
    public class PriceRuleInsuranceHandlerData : TheoryData<double, InsuranceDto>
    {
        public PriceRuleInsuranceHandlerData()
        {
            Add(
                0,
                new InsuranceDto
                {
                    ProductId = 1,
                    ProductTypeId = 2,
                    SalesPrice = 100,
                    InsuranceCost = 0
                }
            );
            Add(
                1000,
                new InsuranceDto
                {
                    ProductId = 1,
                    ProductTypeId = 2,
                    SalesPrice = 500,
                    InsuranceCost = 0 
                }
            );
            Add(
                1000,
                new InsuranceDto
                {
                    ProductId = 1,
                    ProductTypeId = 2,
                    SalesPrice = 600,
                    InsuranceCost = 0
                }
            );
            Add(
                2000,
                new InsuranceDto
                {
                    ProductId = 1, 
                    ProductTypeId = 2, 
                    SalesPrice = 2000, 
                    InsuranceCost = 0
                }
            );
            Add(
                2000,
                new InsuranceDto
                {
                    ProductId = 1,
                    ProductTypeId = 2,
                    SalesPrice = 2100,
                    InsuranceCost = 0
                }
            );
        }
    }
}

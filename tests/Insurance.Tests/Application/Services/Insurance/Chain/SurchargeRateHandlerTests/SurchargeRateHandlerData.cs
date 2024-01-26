using Xunit;

namespace Insurance.Tests.Application.Services.Insurance.Chain.SurchargeRateHandlerTests
{
    public class SurchargeRateHandlerData : TheoryData<ProductInsuranceChainTestDto>
    {
        public SurchargeRateHandlerData()
        {
            Add(new ProductInsuranceChainTestDto
            {
                ProductId = 1,
                ProductTypeId = 1,
                SalesPrice = 1000,
                InsuranceCost = 0,
                Rate = 10,
                ExpectedInsuranceCost = 100
            });
            Add(new ProductInsuranceChainTestDto
            {
                ProductId = 1,
                ProductTypeId = 2,
                SalesPrice = 2000,
                InsuranceCost = 0,
                Rate = 50,
                ExpectedInsuranceCost = 1000
            });
            Add(new ProductInsuranceChainTestDto
            {
                ProductId = 1,
                ProductTypeId = 3,
                SalesPrice = 500,
                InsuranceCost = 0,
                Rate = 20,
                ExpectedInsuranceCost = 100
            });
        }
    }
}

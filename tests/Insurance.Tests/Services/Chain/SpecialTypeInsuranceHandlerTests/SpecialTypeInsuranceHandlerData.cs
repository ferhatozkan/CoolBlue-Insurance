using Insurance.Tests.Services.Chain.PriceRuleInsuranceHandlerTests;
using Xunit;

namespace Insurance.Tests.Services.Chain.SpecialTypeInsuranceHandlerTests
{
    public class SpecialTypeInsuranceHandlerData : TheoryData<ProductInsuranceChainTestDto>
    {
        public SpecialTypeInsuranceHandlerData()
        {
            Add(new ProductInsuranceChainTestDto
            {
                ProductId = 1,
                ProductTypeId = 32,
                SalesPrice = 100,
                InsuranceCost = 0,
                ExpectedInsuranceCost = 500
            });
            Add(new ProductInsuranceChainTestDto
            {
                ProductId = 1,
                ProductTypeId = 21,
                SalesPrice = 500,
                InsuranceCost = 0,
                ExpectedInsuranceCost = 500
            });
            Add(new ProductInsuranceChainTestDto
            {
                ProductId = 1,
                ProductTypeId = 2,
                SalesPrice = 600,
                InsuranceCost = 0,
                ExpectedInsuranceCost = 0
            });
        }
    }
}

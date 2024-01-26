using Xunit;

namespace Insurance.Tests.Services.Chain.PriceRuleInsuranceHandlerTests
{
    public class PriceRuleInsuranceHandlerData : TheoryData<ProductInsuranceChainTestDto>
    {
        public PriceRuleInsuranceHandlerData()
        {
            Add(new ProductInsuranceChainTestDto
            {
                ProductId = 1,
                ProductTypeId = 2,
                SalesPrice = 100,
                InsuranceCost = 0,
                ExpectedInsuranceCost = 0
            });
            Add(new ProductInsuranceChainTestDto
            {
                ProductId = 1,
                ProductTypeId = 2,
                SalesPrice = 500,
                InsuranceCost = 0,
                ExpectedInsuranceCost = 1000
            });
            Add(new ProductInsuranceChainTestDto
            {
                ProductId = 1,
                ProductTypeId = 2,
                SalesPrice = 600,
                InsuranceCost = 0,
                ExpectedInsuranceCost = 1000
            });
            Add(new ProductInsuranceChainTestDto
            {
                ProductId = 1,
                ProductTypeId = 2,
                SalesPrice = 2000,
                InsuranceCost = 0,
                ExpectedInsuranceCost = 2000
            });
            Add(new ProductInsuranceChainTestDto
            {
                ProductId = 1,
                ProductTypeId = 2,
                SalesPrice = 2100,
                InsuranceCost = 0,
                ExpectedInsuranceCost = 2000
            });
        }
    }
}

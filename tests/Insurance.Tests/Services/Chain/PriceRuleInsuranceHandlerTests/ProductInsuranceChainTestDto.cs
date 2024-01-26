namespace Insurance.Tests.Services.Chain.PriceRuleInsuranceHandlerTests
{
    public class ProductInsuranceChainTestDto
    {
        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public double SalesPrice { get; set; }
        public double InsuranceCost { get; set; }
        public double ExpectedInsuranceCost { get; set; }
        public int Rate { get; set; }
    }
}

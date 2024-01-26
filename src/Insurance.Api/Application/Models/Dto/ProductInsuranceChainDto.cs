namespace Insurance.Api.Application.Models.Dto
{
    public class ProductInsuranceChainDto
    {
        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public double SalesPrice { get; set; }
        public double InsuranceCost { get; set; }
    }
}

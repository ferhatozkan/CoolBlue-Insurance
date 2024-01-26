namespace Insurance.Api.Domain.Models.Entities
{
    public class ProductInsuranceRule
    {
        public int? MinSalesPrice { get; set; }
        public int? MaxSalesPrice { get; set; }
        public int InsurancePrice { get; set; }
    }
}

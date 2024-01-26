namespace Insurance.Api.Application.Models.Dto
{
    public class InsuranceDto
    {
        public int ProductId { get; set; }
        public double InsuranceCost { get; set; }
        public int ProductTypeId { get; set; }
        public double SalesPrice { get; set; }
    }
}

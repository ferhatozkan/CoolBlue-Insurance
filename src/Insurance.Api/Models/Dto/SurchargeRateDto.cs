namespace Insurance.Api.Models.Dto
{
    public class SurchargeRateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
        public int ProductTypeId { get; set; }
    }
}

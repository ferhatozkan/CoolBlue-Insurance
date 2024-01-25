namespace Insurance.Api.Models.Entities
{
    public class SurchargeRate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
        public int ProductTypeId { get; set; }
    }
}

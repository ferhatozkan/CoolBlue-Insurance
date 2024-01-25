namespace Insurance.Api.Models.Request
{
    public class UpdateSurchargeRateRequest
    {
        public string Name { get; set; }
        public int Rate { get; set; }
        public int ProductTypeId { get; set; }
    }
}

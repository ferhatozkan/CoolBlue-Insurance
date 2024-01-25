using System.Collections.Generic;

namespace Insurance.Api.Models
{
    public class CartInsuranceDto
    {
        public double TotalInsuranceCost { get; set; }
        public List<InsuranceDto> Products { get; set; }
    }
}

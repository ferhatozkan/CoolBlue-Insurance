using System.Collections.Generic;

namespace Insurance.Api.Models.Dto
{
    public class CartInsuranceDto
    {
        public CartInsuranceDto()
        {
            Products = new List<InsuranceDto>();
        }

        public double TotalInsuranceCost { get; set; }
        public List<InsuranceDto> Products { get; set; }
    }
}

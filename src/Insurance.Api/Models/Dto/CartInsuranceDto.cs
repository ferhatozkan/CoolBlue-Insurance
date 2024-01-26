using System.Collections.Generic;

namespace Insurance.Api.Models.Dto
{
    public class CartInsuranceDto
    {
        public double TotalInsuranceCost { get; set; }
        public List<CartInsuranceItemDto> CartInsuranceItems { get; set; }
    }
}

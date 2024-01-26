using System.Collections.Generic;

namespace Insurance.Api.Presentation.Models.Requests
{
    public class CartInsuranceRequest
    {
        public List<CartInsuranceItem> CartItems { get; set; }
    }
}

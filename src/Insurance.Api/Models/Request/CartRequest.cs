using System.Collections.Generic;

namespace Insurance.Api.Models.Request
{
    public class CartRequest
    {
        public List<CartItem> CartItems { get; set; }
    }
}

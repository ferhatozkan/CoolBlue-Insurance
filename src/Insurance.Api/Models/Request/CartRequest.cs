using System.Collections.Generic;

namespace Insurance.Api.Models.Request
{
    public class CartRequest
    {
        public CartRequest()
        {
            CartItems = new List<CartItem>();
        }
        public List<CartItem> CartItems { get; set; }
    }
}

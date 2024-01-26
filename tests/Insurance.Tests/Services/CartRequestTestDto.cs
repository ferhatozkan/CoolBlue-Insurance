using Insurance.Api.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Tests.Services
{
    public class CartRequestTestDto
    {
        public List<CartItemTestDto> CartItems { get; set; }
        public double TotalInsuranceCost { get; set; }
    }

    public class CartItemTestDto
    {
        public int ProductId { get; set; }
        public int ProductTypeId { get; internal set; }
        public double SalesPrice { get; internal set; }
    }
}

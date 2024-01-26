using System.Collections.Generic;
using Xunit;

namespace Insurance.Tests.Services
{
    public class CalculateCartInsuranceData : TheoryData<CartRequestTestDto>
    {
        public CalculateCartInsuranceData()
        {
            Add(new CartRequestTestDto
            {
                TotalInsuranceCost = 500,
                CartItems = new List<CartItemTestDto>
                { 
                    new CartItemTestDto
                    {
                        ProductId = 1,
                        ProductTypeId = 33
                    }
                }
            });
            Add(new CartRequestTestDto
            {
                TotalInsuranceCost = 0,
                CartItems = new List<CartItemTestDto>
                {
                    new CartItemTestDto
                    {
                        ProductId = 1,
                        ProductTypeId = 2
                    }
                }
            });
        }
    }
}

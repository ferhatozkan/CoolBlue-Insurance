using System.Collections.Generic;
using Insurance.Api.Infrastructure.Clients.Models;
using Xunit;

namespace Insurance.Tests.Application.Services.Insurance
{
    public class CalculateCartInsuranceData : TheoryData<double, List<Product>>
    {
        public CalculateCartInsuranceData()
        {
            Add(
                500,
                new List<Product>
                {
                    new Product
                    {
                        Id = 1,
                        ProductTypeId = 33
                    }
                }
            );
            Add(
                0,
                new List<Product>
                {
                    new Product
                    {
                        Id = 1,
                        ProductTypeId = 2
                    }
                });
        }
    }
}
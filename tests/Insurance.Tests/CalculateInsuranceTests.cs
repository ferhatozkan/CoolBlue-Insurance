using System;
using System.Collections.Generic;
using Insurance.Api.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Xunit;

namespace Insurance.Tests
{
    public class CalculateInsuranceTests : IClassFixture<ControllerTestFixture>
    {
        private readonly ControllerTestFixture _fixture;

        public CalculateInsuranceTests(ControllerTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 1000)]
        [InlineData(3, 2000)]
        [InlineData(9, 1000)]
        public void GivenProductTypeNotSpecial_ShouldAddExpectedEurosToInsuranceCost(int productId, float expectedInsuranceValue)
        {
            var dto = new HomeController.InsuranceDto
            {
                ProductId = productId,
            };
            var sut = new HomeController();

            var result = sut.CalculateInsurance(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Theory]
        [InlineData(4, 500)]
        [InlineData(5, 1500)]
        [InlineData(6, 2500)]
        public void GivenProductTypeSpecial_ShouldAddExpectedEurosToInsuranceCost(int productId, float expectedInsuranceValue)
        {
            var dto = new HomeController.InsuranceDto
            {
                ProductId = productId,
            };
            var sut = new HomeController();

            var result = sut.CalculateInsurance(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Theory]
        [InlineData(7)]
        [InlineData(8)]
        public void GivenProductTypeCanBeInsuredFalse_ShouldAdd0EurosToInsuranceCost(int productId)
        {
            const float expectedInsuranceValue = 0;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = productId,
            };
            var sut = new HomeController();

            var result = sut.CalculateInsurance(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }
    }

    public class ControllerTestFixture : IDisposable
    {
        private readonly IHost _host;

        public ControllerTestFixture()
        {
            _host = new HostBuilder()
                   .ConfigureWebHostDefaults(
                        b => b.UseUrls("http://localhost:5002")
                              .UseStartup<ControllerTestStartup>()
                    )
                   .Build();

            _host.Start();
        }

        public void Dispose() => _host.Dispose();
    }

    public class ControllerTestStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            var productDictionary = new Dictionary<int, object>
            {
                { 1, new{
                            id = 1,
                            name = "Test Product with price 300 productTypeId NotSpecial",
                            productTypeId = 1,
                            salesPrice = 300
                            }
                },
                { 2, new{
                            id = 2,
                            name = "Test Product with price 700 productTypeId NotSpecial",
                            productTypeId = 1,
                            salesPrice = 700
                            }
                },
                { 3, new{
                            id = 3,
                            name = "Test Product with price 2200 productTypeId NotSpecial",
                            productTypeId = 1,
                            salesPrice = 2200
                            }
                },
                { 4, new{
                            id = 4,
                            name = "Test Product with price 200 productTypeId special",
                            productTypeId = 2,
                            salesPrice = 200
                            }
                },
                { 5, new{
                            id = 5,
                            name = "Test Product with price 600 productTypeId special",
                            productTypeId = 2,
                            salesPrice = 600
                            }
                },
                { 6, new{
                            id = 6,
                            name = "Test Product with price 2000 productTypeId special",
                            productTypeId = 2,
                            salesPrice = 2000
                            }
                },
                { 7, new{
                            id = 7,
                            name = "Test Product with price 100 productTypeId CanBeInsured False",
                            productTypeId = 3,
                            salesPrice = 100
                            }
                },
                { 8, new{
                            id = 8,
                            name = "Test Product with price 2100 productTypeId CanBeInsured False",
                            productTypeId = 3,
                            salesPrice = 2100
                            }
                },
                { 9, new{
                            id = 9,
                            name = "Test Product with price 500 productTypeId NotSpecial",
                            productTypeId = 1,
                            salesPrice = 500
                            }
                }
            };

            app.UseRouting();
            app.UseEndpoints(
                ep =>
                {
                    ep.MapGet(
                        "products/{id:int}",
                        context =>
                        {
                            int productId = int.Parse((string)context.Request.RouteValues["id"]);
                            var product = productDictionary.GetValueOrDefault(productId);
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(product));
                        }
                    );
                    ep.MapGet(
                        "product_types",
                        context =>
                        {
                            var productTypes = new[]
                                               {
                                                   new
                                                   {
                                                       id = 1,
                                                       name = "CanBeInsured Simple Type",
                                                       canBeInsured = true
                                                   },
                                                   new
                                                   {
                                                       id = 2,
                                                       name = "Smartphones",
                                                       canBeInsured = true
                                                   },
                                                   new
                                                   {
                                                       id = 3,
                                                       name = "CanBeInsured False Simple Type",
                                                       canBeInsured = false
                                                   }
                                               };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(productTypes));
                        }
                    );
                }
            );
        }
    }
}
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

        [Fact]
        public void GivenSalesPriceBetween500And2000EurosProductType1_ShouldAdd1000EurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 1000;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = 1,
            };
            var sut = new HomeController();

            var result = sut.CalculateInsurance(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public void GivenSalesPriceLessThan500EurosProductType1_ShouldAdd0EurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 0;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = 2,
            };
            var sut = new HomeController();

            var result = sut.CalculateInsurance(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public void GivenSalesPriceIsGreaterThan2000EurosProductType1_ShouldAdd2000EurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 2000;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = 3,
            };
            var sut = new HomeController();

            var result = sut.CalculateInsurance(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public void GivenSalesPriceIsLessThan500EurosProductType2_ShouldAdd500EurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 500;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = 4,
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
                            name = "Test Product with price 500 productTypeId 1",
                            productTypeId = 1,
                            salesPrice = 750
                            }
                },
                { 2, new{
                            id = 2,
                            name = "Test Product with price 300 productTypeId 1",
                            productTypeId = 1,
                            salesPrice = 300
                            }
                },
                { 3, new{
                            id = 3,
                            name = "Test Product with price 2200 productTypeId 1",
                            productTypeId = 1,
                            salesPrice = 2200
                            }
                },
                { 4, new{
                            id = 4,
                            name = "Test Product with price 200 productTypeId 2",
                            productTypeId = 2,
                            salesPrice = 200
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
                                                       name = "SmartPhones",
                                                       canBeInsured = true
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
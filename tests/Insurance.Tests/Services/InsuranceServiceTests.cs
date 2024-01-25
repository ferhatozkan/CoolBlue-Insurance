using Insurance.Api.Clients;
using Insurance.Api.Clients.Models;
using Insurance.Api.Models;
using Insurance.Api.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Services
{
    public class InsuranceServiceTests
    {

        private Mock<IProductApiClient> _productApiClient;
        private InsuranceService _insuranceService;
        public InsuranceServiceTests()
        {
            _productApiClient = new Mock<IProductApiClient>();
            _insuranceService = new InsuranceService(_productApiClient.Object);
        }

        [Theory]
        [InlineData("ProductTypeCantBeInsured", 0)]
        [InlineData("PriceRange(0-500)-NonSpecialType", 0)]
        [InlineData("PriceRange(500-1000)-InclusiveLowerLimit-NonSpecialType", 1000)]
        [InlineData("PriceRange(500-1000)-InclusiveLowerLimit-Price500-NonSpecialType", 1000)]
        [InlineData("PriceRange(2000-Infinity)-InclusiveLowerLimit-NonSpecialType", 2000)]
        [InlineData("PriceRange(2000-Infinity)-InclusiveLowerLimit-Price2000-NonSpecialType", 2000)]
        [InlineData("PriceRange(0-500)-SpecialType", 500)]
        [InlineData("PriceRange(500-1000)-InclusiveLowerLimit-SpecialType", 1500)]
        [InlineData("PriceRange(500-1000)-InclusiveLowerLimit-Price500-SpecialType", 1500)]
        [InlineData("PriceRange(2000-Infinity)-InclusiveLowerLimit-SpecialType", 2500)]
        [InlineData("PriceRange(2000-Infinity)-InclusiveLowerLimit-Price2000-SpecialType", 2500)]
        public async Task GivenUseCaseProduct_CalculateProductInsuranceShouldReturnExpectedInsurance(string testCase, int expectedInsurance)
        {
            var data = GetProduct().GetValueOrDefault(testCase);
            var product = data.Item1;
            var productType = data.Item2;

            _productApiClient.Setup(client => client.GetProduct(It.IsAny<int>()))
                .Returns(Task.FromResult(product));

            _productApiClient.Setup(client => client.GetProductType(It.IsAny<int>()))
                .Returns(Task.FromResult(productType));

            var result = await _insuranceService.CalculateProductInsurance(1);
            Assert.NotNull(result);
            Assert.Equal(expected: expectedInsurance, result.InsuranceCost);
        }

        [Fact]
        public async Task GivenGetProductThrowsException_CalculateProductInsuranceShouldThrowException()
        {
            _productApiClient.Setup(client => client.GetProduct(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _insuranceService.CalculateProductInsurance(1));
        }

        [Fact]
        public async Task GivenGetProductSuccessAndGetProductTypeThrowsException_CalculateProductInsuranceShouldThrowException()
        {
            var product = new ProductDto
            {
                Id = 1,
                Name = "Apple iPod",
                SalesPrice = 229,
                ProductTypeId = 12
            };

            _productApiClient.Setup(client => client.GetProduct(It.IsAny<int>()))
                .Returns(Task.FromResult(product));

            _productApiClient.Setup(client => client.GetProductType(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _insuranceService.CalculateProductInsurance(1));
        }

        [Theory]
        [InlineData("CartProductTypesCantBeInsured", 0)]
        [InlineData("CartThreeProductsWithPriceRange(500-1000)-InclusiveLowerLimit-NonSpecialType", 3000)]
        [InlineData("CartProductsWithPriceRange(2000-Infinity)-InclusiveLowerLimit-SpecialTypeAndPriceRange(500-1000)-InclusiveLowerLimit-NonSpecialType", 3500)]
        public async Task GivenUseCaseCartProducts_CalculateCartInsuranceShouldReturnExpectedInsurance(string testCase, int expectedCartInsurance)
        {
            var data = GetCartProducts().GetValueOrDefault(testCase);

            var products = data.Item1;
            var productTypes = data.Item2;

            foreach (var product in products)
            {
                _productApiClient.Setup(client => client.GetProduct(product.Id))
                    .Returns(Task.FromResult(product));
            }

            foreach (var productType in productTypes)
            {
                _productApiClient.Setup(client => client.GetProductType(productType.Id))
                    .Returns(Task.FromResult(productType));
            }

            var result = await _insuranceService.CalculateCartInsurance(products.Select(p => p.Id).ToList());

            Assert.NotNull(result);
            Assert.Equal(expected: expectedCartInsurance, result.TotalInsuranceCost);
        }

        [Fact]
        public async Task GivenGetProductThrowsException_CalculateCartInsuraceShouldThrowException()
        {
            _productApiClient.Setup(client => client.GetProduct(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            var productIds = new List<int> { 1, 2 };
            await Assert.ThrowsAsync<Exception>(async () => await _insuranceService.CalculateCartInsurance(productIds));
        }

        [Fact]
        public async Task GivenGetProductSuccessAndGetProductTypeThrowsException_CalculateCartInsuranceShouldThrowException()
        {
            var product = new ProductDto
            {
                Id = 1,
                Name = "Apple iPod",
                SalesPrice = 229,
                ProductTypeId = 12
            };

            _productApiClient.Setup(client => client.GetProduct(It.IsAny<int>()))
                .Returns(Task.FromResult(product));

            _productApiClient.Setup(client => client.GetProductType(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            var productIds = new List<int> { 1, 2 };
            await Assert.ThrowsAsync<Exception>(async () => await _insuranceService.CalculateCartInsurance(productIds));
        }

        private Dictionary<string, Tuple<ProductDto, ProductTypeDto>> GetProduct()
        {
            var data = new Dictionary<string, Tuple<ProductDto, ProductTypeDto>>
            {
                {
                    "ProductTypeCantBeInsured",
                    Tuple.Create(
                        new ProductDto
                        {
                            Id = 1,
                            Name = "Product",
                            ProductTypeId = 1,
                            SalesPrice = 2500
                        },
                        new ProductTypeDto
                        {
                            Id = 1,
                            Name = "CanBeInsured False Type",
                            CanBeInsured = false
                        })
                },
                {
                    "PriceRange(0-500)-NonSpecialType",
                    Tuple.Create(
                        new ProductDto
                        {
                            Id = 1,
                            Name = "Product",
                            ProductTypeId = 1,
                            SalesPrice = 300
                        },
                        new ProductTypeDto
                        {
                            Id = 1,
                            Name = "CanBeInsured Simple Type",
                            CanBeInsured = true
                        })
                },
                {
                    "PriceRange(500-1000)-InclusiveLowerLimit-NonSpecialType",
                    Tuple.Create(
                        new ProductDto
                        {
                            Id = 1,
                            Name = "Product",
                            ProductTypeId = 1,
                            SalesPrice = 600
                        },
                        new ProductTypeDto
                        {
                            Id = 1,
                            Name = "CanBeInsured Simple Type",
                            CanBeInsured = true
                        })
                },
                {
                    "PriceRange(500-1000)-InclusiveLowerLimit-Price500-NonSpecialType",
                    Tuple.Create(
                        new ProductDto
                        {
                            Id = 1,
                            Name = "Product",
                            ProductTypeId = 1,
                            SalesPrice = 500
                        },
                        new ProductTypeDto
                        {
                            Id = 1,
                            Name = "CanBeInsured Simple Type",
                            CanBeInsured = true
                        })
                },
                {
                    "PriceRange(2000-Infinity)-InclusiveLowerLimit-NonSpecialType",
                    Tuple.Create(
                        new ProductDto
                        {
                            Id = 1,
                            Name = "Product",
                            ProductTypeId = 1,
                            SalesPrice = 2100
                        },
                        new ProductTypeDto
                        {
                            Id = 1,
                            Name = "CanBeInsured Simple Type",
                            CanBeInsured = true
                        })
                },
                {
                    "PriceRange(2000-Infinity)-InclusiveLowerLimit-Price2000-NonSpecialType",
                    Tuple.Create(
                        new ProductDto
                        {
                            Id = 1,
                            Name = "Product",
                            ProductTypeId = 1,
                            SalesPrice = 2000
                        },
                        new ProductTypeDto
                        {
                            Id = 1,
                            Name = "CanBeInsured Simple Type",
                            CanBeInsured = true
                        })
                },
                {
                    "PriceRange(0-500)-SpecialType",
                    Tuple.Create(
                        new ProductDto
                        {
                            Id = 1,
                            Name = "Product",
                            ProductTypeId = 1,
                            SalesPrice = 300
                        },
                        new ProductTypeDto
                        {
                            Id = 1,
                            Name = "Smartphones",
                            CanBeInsured = true
                        })
                },
                {
                    "PriceRange(500-1000)-InclusiveLowerLimit-SpecialType",
                    Tuple.Create(
                        new ProductDto
                        {
                            Id = 1,
                            Name = "Product",
                            ProductTypeId = 1,
                            SalesPrice = 600
                        },
                        new ProductTypeDto
                        {
                            Id = 1,
                            Name = "Smartphones",
                            CanBeInsured = true
                        })
                },
                {
                    "PriceRange(500-1000)-InclusiveLowerLimit-Price500-SpecialType",
                    Tuple.Create(
                        new ProductDto
                        {
                            Id = 1,
                            Name = "Product",
                            ProductTypeId = 1,
                            SalesPrice = 500
                        },
                        new ProductTypeDto
                        {
                            Id = 1,
                            Name = "Smartphones",
                            CanBeInsured = true
                        })
                },
                {
                    "PriceRange(2000-Infinity)-InclusiveLowerLimit-SpecialType",
                    Tuple.Create(
                        new ProductDto
                        {
                            Id = 1,
                            Name = "Product",
                            ProductTypeId = 1,
                            SalesPrice = 2100
                        },
                        new ProductTypeDto
                        {
                            Id = 1,
                            Name = "Smartphones",
                            CanBeInsured = true
                        })
                },
                {
                    "PriceRange(2000-Infinity)-InclusiveLowerLimit-Price2000-SpecialType",
                    Tuple.Create(
                        new ProductDto
                        {
                            Id = 1,
                            Name = "Product",
                            ProductTypeId = 1,
                            SalesPrice = 2000
                        },
                        new ProductTypeDto
                        {
                            Id = 1,
                            Name = "Smartphones",
                            CanBeInsured = true
                        })
                }
            };

            return data;
        }

        private Dictionary<string, Tuple<List<ProductDto>, List<ProductTypeDto>>> GetCartProducts()
        {
            var data = new Dictionary<string, Tuple<List<ProductDto>, List<ProductTypeDto>>>
            {
                {
                    "CartProductTypesCantBeInsured",
                    Tuple.Create(
                        new List<ProductDto>
                        {
                            new ProductDto
                            {
                                Id = 1,
                                Name = "Product",
                                ProductTypeId = 1,
                                SalesPrice = 2500
                            },
                            new ProductDto
                            {
                                Id = 2,
                                Name = "Product",
                                ProductTypeId = 2,
                                SalesPrice = 700
                            }
                        },
                        new List<ProductTypeDto>
                        {
                            new ProductTypeDto
                            {
                                Id = 1,
                                Name = "CanBeInsured False Type",
                                CanBeInsured = false
                            },
                            new ProductTypeDto
                            {
                                Id = 2,
                                Name = "CanBeInsured False Type",
                                CanBeInsured = false
                            }
                        })
                },
                {
                    "CartThreeProductsWithPriceRange(500-1000)-InclusiveLowerLimit-NonSpecialType",
                    Tuple.Create(
                       new List<ProductDto>
                        {
                            new ProductDto
                            {
                                Id = 1,
                                Name = "Product",
                                ProductTypeId = 1,
                                SalesPrice = 600
                            },
                            new ProductDto
                            {
                                Id = 2,
                                Name = "Product",
                                ProductTypeId = 1,
                                SalesPrice = 700
                            },
                            new ProductDto
                            {
                                Id = 3,
                                Name = "Product",
                                ProductTypeId = 1,
                                SalesPrice = 800
                            }
                        },
                        new List<ProductTypeDto>
                        {
                            new ProductTypeDto
                            {
                                Id = 1,
                                Name = "CanBeInsured Simple Type",
                                CanBeInsured = true
                            },
                        })
                },
                {
                    "CartProductsWithPriceRange(2000-Infinity)-InclusiveLowerLimit-SpecialTypeAndPriceRange(500-1000)-InclusiveLowerLimit-NonSpecialType",
                    Tuple.Create(
                          new List<ProductDto>
                        {
                            new ProductDto
                            {
                                Id = 1,
                                Name = "Product",
                                ProductTypeId = 1,
                                SalesPrice = 2100
                            },
                            new ProductDto
                            {
                                Id = 2,
                                Name = "Product",
                                ProductTypeId = 2,
                                SalesPrice = 700
                            }
                        },
                        new List<ProductTypeDto>
                        {
                            new ProductTypeDto
                            {
                                Id = 1,
                                Name = "Smartphones",
                                CanBeInsured = true
                            },
                            new ProductTypeDto
                            {
                                Id = 2,
                                Name = "CanBeInsured Simple Type",
                                CanBeInsured = true
                            }
                        })
                }
            };

            return data;
        }
    }
}

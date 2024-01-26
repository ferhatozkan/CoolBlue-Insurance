using Insurance.Api.Clients;
using Insurance.Api.Clients.Configuration;
using Insurance.Api.Exceptions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Clients
{
    public class ProductApiClientTests
    {
        private ProductApiClientConfiguration _productApiClientConfiguration;

        private IProductApiClient _productApiClient { get; set; }

        private Mock<IHttpClientFactory> _httpClientFactoryMock;

        public ProductApiClientTests()
        {
            _productApiClientConfiguration = new ProductApiClientConfiguration
            {
                Url = "http://localhost"
            };

            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        }

        [Fact]
        public async Task GivenProductTypeByIdReturns200_ShouldReturnProductType()
        {
            var content = new StringContent("{\r\n  \"id\": 33,\r\n  \"name\": \"Digital cameras\",\r\n  \"canBeInsured\": true\r\n}");
            MockHttpClientCreator(HttpStatusCode.OK, content);

            var result = await _productApiClient.GetProductType(33);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GivenProductTypeByIdReturns404_ShouldThrowNotFoundException()
        {
            var content = new StringContent("{\r\n  \"type\": \"https://tools.ietf.org/html/rfc7231#section-6.5.4\",\r\n  \"title\": \"Not Found\",\r\n  \"status\": 404\r\n \"traceId\": 1\r\n}");
            MockHttpClientCreator(HttpStatusCode.NotFound, content);

            await Assert.ThrowsAsync<NotFoundException>(async() => await _productApiClient.GetProductType(100));
        }

        [Fact]
        public async Task GivenProductByIdReturns200_ShouldReturnProduct()
        {
            var content = new StringContent("{\r\n  \"id\": 572770,\r\n  \"name\": \"Samsung WW8\",\r\n  \"salesPrice\": 475,\r\n \"productTypeId\": 124\r\n }");
            MockHttpClientCreator(HttpStatusCode.OK, content);

            var result = await _productApiClient.GetProduct(572770);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GivenProductByIdReturns404_ShouldThrowNotFoundException()
        {
            var content = new StringContent("{\r\n  \"type\": \"https://tools.ietf.org/html/rfc7231#section-6.5.4\",\r\n  \"title\": \"Not Found\",\r\n  \"status\": 404\r\n \"traceId\": 1\r\n}");
            MockHttpClientCreator(HttpStatusCode.NotFound, content);

            await Assert.ThrowsAsync<NotFoundException>(async () => await _productApiClient.GetProductType(100));
        }

        private void MockHttpClientCreator(HttpStatusCode statusCode, HttpContent content)
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();

            var mockedProtected = mockMessageHandler.Protected();

            mockedProtected
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = statusCode,
                    Content = content
                });

            var client = new HttpClient(mockMessageHandler.Object)
            {
                BaseAddress = new Uri(_productApiClientConfiguration.Url)
            };

            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            _productApiClient = new ProductApiClient(Options.Create(_productApiClientConfiguration), _httpClientFactoryMock.Object);
        }
    }
}

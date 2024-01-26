using Insurance.Api.Application.Extensions;
using Insurance.Api.Application.Models.Dto;
using Insurance.Api.Presentation.Models.Requests;
using Insurance.Api.Presentation.Models.Responses;
using System.Net;
using Xunit;

namespace Insurance.IntegrationTests
{
    public class ProductInsuranceTests : BaseIntegrationTest, IDisposable
    {
        public List<int> SurchargeIds { get; set; }

        public async void Dispose()
        {
            if (SurchargeIds != null && SurchargeIds.Any())
            {
                foreach (int id in SurchargeIds)
                {
                    await SendAsync(HttpMethod.Delete, $"api/surcharge-rates/{id}");
                }
            }
        }

        [Theory]
        [MemberData(nameof(CalculateInsuranceData))]
        public async Task WhenCalculateInsuranceTestsCalledWithValidProducts_ShouldReturn200Response(int productId, double expectedResult)
        {
            var response = await SendAsync(HttpMethod.Get, $"api/insurance/product/{productId}");

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var productInsuranceDto = JsonHelper.DeserializeCaseInsensitive<ProductInsuranceDto>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(productInsuranceDto);
            Assert.Equal(expectedResult, productInsuranceDto.InsuranceCost);
            Assert.Equal(productId, productInsuranceDto.ProductId);
        }

        [Fact]
        public async Task WhenCalculateInsuranceTestsCalledWithNonExistingProductId_ShouldReturn404Response()
        {
            var response = await SendAsync(HttpMethod.Get, $"api/insurance/product/{-1}");
            
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var errorMessage = JsonHelper.DeserializeCaseInsensitive<ErrorMessageResponse>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(errorMessage);
            Assert.Equal("Product with id -1 not found", errorMessage.ErrorMessages[0].Message);
        }

        [Fact]
        public async Task WhenCalculateInsuranceTestsCalledWithSurchargeRate_ShouldReturn200Response()
        {
            SurchargeIds = new List<int>();
            var successCreateResponse = await CreateSurchargeRate(new CreateSurchargeRateRequest{ Name = "test", Rate = 10, ProductTypeId = 124 });
            SurchargeIds.Add(successCreateResponse.Id);

            var response = await SendAsync(HttpMethod.Get, $"api/insurance/product/735246");
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var productInsuranceDto = JsonHelper.DeserializeCaseInsensitive<ProductInsuranceDto>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(productInsuranceDto);
            Assert.Equal(1069.9d, productInsuranceDto.InsuranceCost);
            Assert.Equal(735246, productInsuranceDto.ProductId);
        }

        public static IEnumerable<object[]> CalculateInsuranceData =>
             new List<object[]>
             {
                    new object[] { 805073, 0 },
                    new object[] { 735246, 1000 },
                    new object[] { 828519, 500 },
                    new object[] { 859366, 1500 }
             };
    }
}

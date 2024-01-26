using Insurance.Api.Application.Extensions;
using Insurance.Api.Application.Models.Dto;
using Insurance.Api.Presentation.Models.Requests;
using Insurance.Api.Presentation.Models.Responses;
using System.Net;
using Xunit;

namespace Insurance.IntegrationTests
{
    public class SurchargeRateTests : BaseIntegrationTest, IDisposable
    {
        public List<int> SurchargeIds { get; set; }

        public async void Dispose()
        {
            if(SurchargeIds != null && SurchargeIds.Any())
            {
                foreach(int id in SurchargeIds)
                {
                    await SendAsync(HttpMethod.Delete, $"api/surcharge-rates/{id}");
                }
            }
        }

        [Fact]
        public async Task WhenGetAllCalled_ShouldReturn200SuccessResponse()
        {
            var getAllResponse = await SendAsync(HttpMethod.Get, $"api/surcharge-rates");

            Assert.NotNull(getAllResponse);
            Assert.Equal(HttpStatusCode.OK, getAllResponse.StatusCode);

            var surchargeRateDtos = JsonHelper.DeserializeCaseInsensitive<List<SurchargeRateDto>>(await getAllResponse.Content.ReadAsStringAsync());

            Assert.NotNull(surchargeRateDtos);
        }

        [Fact]
        public async Task WhenGetAllCalledWithCreate_ShouldReturn200SuccessResponse()
        {
            var surchargeRule1 = new CreateSurchargeRateRequest
            {
                ProductTypeId = 1,
                Name = "Test Rate 1",
                Rate = 10
            };
            var surchargeRule2 = new CreateSurchargeRateRequest
            {
                ProductTypeId = 2,
                Name = "Test Rate 2",
                Rate = 20
            };


            SurchargeIds = new List<int>();
            var firstSurchargeCreateResponse = await CreateSurchargeRate(surchargeRule1);
            SurchargeIds.Add(firstSurchargeCreateResponse.Id);
            
            var secondSurchargeCreateResponse = await CreateSurchargeRate(surchargeRule2);
            SurchargeIds.Add(secondSurchargeCreateResponse.Id);

            var expected = new List<SurchargeRateDto>()
            {
                firstSurchargeCreateResponse,
                secondSurchargeCreateResponse
            };

            Thread.Sleep(100);
            var getAllResponse = await SendAsync(HttpMethod.Get, $"api/surcharge-rates");

            Assert.NotNull(getAllResponse);
            Assert.Equal(HttpStatusCode.OK, getAllResponse.StatusCode);

            var surchargeRateDtos = JsonHelper.DeserializeCaseInsensitive<List<SurchargeRateDto>>(await getAllResponse.Content.ReadAsStringAsync());

            Assert.NotNull(surchargeRateDtos);
            
            Assert.NotStrictEqual(expected, surchargeRateDtos);
        }

        [Fact]
        public async Task WhenGetByIdCalledWithCreate_ShouldReturn200SuccessResponse()
        {
            var surchargeRule = new CreateSurchargeRateRequest
            {
                ProductTypeId = 1,
                Name = "Test Rate 1",
                Rate = 10
            };

            SurchargeIds = new List<int>();
            var surchargeRate = await CreateSurchargeRate(surchargeRule);
            SurchargeIds.Add(surchargeRate.Id);

            Thread.Sleep(100);
            var getByIdResponse = await SendAsync(HttpMethod.Get, $"api/surcharge-rates/{surchargeRate.Id}");

            Assert.NotNull(getByIdResponse);
            Assert.Equal(HttpStatusCode.OK, getByIdResponse.StatusCode);

            var getByIdSurchargeRate = JsonHelper.DeserializeCaseInsensitive<SurchargeRateDto>(await getByIdResponse.Content.ReadAsStringAsync());

            Assert.NotNull(getByIdSurchargeRate);

            Assert.Equal(surchargeRate.ProductTypeId, getByIdSurchargeRate.ProductTypeId);
            Assert.Equal(surchargeRate.Name, getByIdSurchargeRate.Name);
            Assert.Equal(surchargeRate.Rate, getByIdSurchargeRate.Rate);
        }

        [Fact]
        public async Task WhenGetByIdCalledWithNonExistingProductId_ShouldReturn404NotFoundResponse()
        {
            var productId = 2000;
            var getByIdResponse = await SendAsync(HttpMethod.Get, $"api/surcharge-rates/{productId}");

            Assert.NotNull(getByIdResponse);
            Assert.Equal(HttpStatusCode.NotFound, getByIdResponse.StatusCode);

            var errorMessage = JsonHelper.DeserializeCaseInsensitive<ErrorMessageResponse>(await getByIdResponse.Content.ReadAsStringAsync());

            Assert.NotNull(errorMessage);
            Assert.Equal($"SurchargeRate with id {productId} not found", errorMessage.ErrorMessages[0].Message);
        }


        [Fact]
        public async Task WhenCreateCalled_Return200SuccessResponse()
        {
            var surchargeRule = new CreateSurchargeRateRequest
            {
                ProductTypeId = 1,
                Name = "Test Rate 1",
                Rate = 10
            };

            SurchargeIds = new List<int>();
            var getByIdSurchargeRate = await CreateSurchargeRate(surchargeRule);
            SurchargeIds.Add(getByIdSurchargeRate.Id);

            Assert.NotNull(getByIdSurchargeRate);

            Assert.Equal(surchargeRule.ProductTypeId, getByIdSurchargeRate.ProductTypeId);
            Assert.Equal(surchargeRule.Name, getByIdSurchargeRate.Name);
            Assert.Equal(surchargeRule.Rate, getByIdSurchargeRate.Rate);
        }

        [Fact]
        public async Task WhenCreateCalledOnExistingProductType_Return400BadRequestResponse()
        {
            var surchargeRule = new CreateSurchargeRateRequest
            {
                ProductTypeId = 1,
                Name = "Test Rate 1",
                Rate = 10
            };

            SurchargeIds = new List<int>();
            var successCreateResponse = await CreateSurchargeRate(surchargeRule);
            SurchargeIds.Add(successCreateResponse.Id);
            
            Thread.Sleep(10);

            var failedCreateResponse = await SendAsync(HttpMethod.Post, $"api/surcharge-rates", surchargeRule);

            Assert.NotNull(failedCreateResponse);
            Assert.Equal(HttpStatusCode.BadRequest, failedCreateResponse.StatusCode);
        }
    }
}

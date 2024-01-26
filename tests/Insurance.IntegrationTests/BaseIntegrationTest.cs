using Insurance.Api.Application.Extensions;
using Insurance.Api.Application.Models.Dto;
using Insurance.Api.Presentation.Models.Requests;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Insurance.IntegrationTests
{
    public class BaseIntegrationTest 
    {
        protected readonly HttpClient _client;
        private const string BASE_URL = "http://localhost:8081/";
        public BaseIntegrationTest()
        {
            var baseUrl = Environment.GetEnvironmentVariable("API_URL");

            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl ?? BASE_URL);
        }

        protected async Task<HttpResponseMessage> SendAsync(HttpMethod httpMethod, string path, object request = null)
        {
            var requestMessage = new HttpRequestMessage(httpMethod, path)
            {
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json)
            };

            var response = await _client.SendAsync(requestMessage);

            return response;
        }

        protected async Task<SurchargeRateDto> CreateSurchargeRate(CreateSurchargeRateRequest request)
        {
            var createResponse = await SendAsync(HttpMethod.Post, $"api/surcharge-rates", request);

            Assert.NotNull(createResponse);
            Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);

            var response = JsonHelper.DeserializeCaseInsensitive<SurchargeRateDto>(await createResponse.Content.ReadAsStringAsync());
            return response;
        }
    }
}

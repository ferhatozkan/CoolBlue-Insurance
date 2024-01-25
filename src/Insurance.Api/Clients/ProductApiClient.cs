using System.Net.Http;
using System;
using Insurance.Api.Clients.Configuration;
using Microsoft.Extensions.Options;
using Insurance.Api.Clients.Models;
using System.Threading.Tasks;
using System.Text.Json;

namespace Insurance.Api.Clients
{
    public class ProductApiClient : IProductApiClient
    {
        private readonly HttpClient _client;
        public ProductApiClient(IOptions<ProductApiClientConfiguration> configuration, IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri(configuration.Value.Url);
        }

        public async Task<ProductTypeDto> GetProductType(int productTypeId)
        {
            var response = await _client.GetAsync($"/product_types/{productTypeId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Product Type with id {productTypeId} not found");
            }

            var productTypeDto = JsonSerializer.Deserialize<ProductTypeDto>(await response.Content.ReadAsStringAsync());

            return productTypeDto;
        }

        public async Task<ProductDto> GetProduct(int productId)
        {
            var response = await _client.GetAsync($"/products/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Product with id {productId} not found");
            }

            var productDto = JsonSerializer.Deserialize<ProductDto>(await response.Content.ReadAsStringAsync());

            return productDto;
        }
    }
}

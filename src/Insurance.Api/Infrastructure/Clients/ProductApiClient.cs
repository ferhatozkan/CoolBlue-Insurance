using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Insurance.Api.Application.Clients;
using Insurance.Api.Application.Exceptions;
using Insurance.Api.Infrastructure.Clients.Configuration;
using Insurance.Api.Infrastructure.Clients.Models;
using Microsoft.Extensions.Options;

namespace Insurance.Api.Infrastructure.Clients
{
    public class ProductApiClient : IProductApiClient
    {
        private readonly HttpClient _client;
        public ProductApiClient(IOptions<ProductApiClientConfiguration> configuration, IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri(configuration.Value.Url);
        }

        public async Task<ProductType> GetProductType(int productTypeId)
        {
            var response = await _client.GetAsync($"/product_types/{productTypeId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new NotFoundException($"Product Type with id {productTypeId} not found");
            }

            var productTypeDto = JsonSerializer.Deserialize<ProductType>(await response.Content.ReadAsStringAsync());

            return productTypeDto;
        }

        public async Task<Product> GetProduct(int productId)
        {
            var response = await _client.GetAsync($"/products/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new NotFoundException($"Product with id {productId} not found");
            }

            var productDto = JsonSerializer.Deserialize<Product>(await response.Content.ReadAsStringAsync());

            return productDto;
        }
    }
}

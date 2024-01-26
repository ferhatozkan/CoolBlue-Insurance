using System.Text.Json.Serialization;

namespace Insurance.Api.Infrastructure.Clients.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("salesPrice")]
        public double SalesPrice { get; set; }

        [JsonPropertyName("productTypeId")]
        public int ProductTypeId { get; set; }
    }
}

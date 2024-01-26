using System.Text.Json.Serialization;

namespace Insurance.Api.Infrastructure.Clients.Models
{
    public class ProductType
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("canBeInsured")]
        public bool CanBeInsured { get; set; }
    }
}

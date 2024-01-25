using System.Text.Json.Serialization;

namespace Insurance.Api.Clients.Models
{
    public class ProductTypeDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("canBeInsured")]
        public bool CanBeInsured { get; set; }
    }
}

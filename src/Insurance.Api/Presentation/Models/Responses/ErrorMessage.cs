using System.Text.Json.Serialization;

namespace Insurance.Api.Presentation.Models.Responses
{
    public record ErrorMessage([property: JsonPropertyName("message")] string Message);
}

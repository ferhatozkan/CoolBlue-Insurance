using System.Text.Json;

namespace Insurance.Api.Application.Extensions
{
    public class JsonHelper
    {
        public static T DeserializeCaseInsensitive<T>(string content)
        {
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}

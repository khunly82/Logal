using Logal.Enums;
using System.Text.Json.Serialization;

namespace Logal.DTO
{
    public class ResponseDTO
    {
        public string Message { get; set; } = null!;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("State")]
        public Status Status  { get; set; }

    }
}

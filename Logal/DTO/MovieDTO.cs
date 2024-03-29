using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace Logal.DTO
{
    public class MovieDTO
    {
        public string _id { get; set; } = null!;
        public string title { get; set; } = null!;
        public int rating { get; set; }

        [JsonPropertyName("lock")]
        public string? verrou { get; set;}
    }
}

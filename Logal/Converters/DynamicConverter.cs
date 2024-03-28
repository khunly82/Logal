
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Logal.Converters
{
    public class DynamicConverter : System.Text.Json.Serialization.JsonConverter<dynamic>
    {
        public override dynamic? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            string json = doc.RootElement.GetRawText();
            return JsonConvert.DeserializeObject<dynamic>(json);
        }

        public override void Write(Utf8JsonWriter writer, dynamic value, JsonSerializerOptions options)
        {
            //throw new NotImplementedException();
        }
    }
}

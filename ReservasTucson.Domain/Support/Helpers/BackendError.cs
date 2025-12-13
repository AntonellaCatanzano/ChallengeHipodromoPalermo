using System.Text.Json;
using System.Text.Json.Serialization;


namespace ReservasTucson.Domain.Support.Helpers
{
    public class BackendError
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

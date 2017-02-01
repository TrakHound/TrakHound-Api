using Newtonsoft.Json;

namespace TrakHound.Api.v2.Data
{
    public class Component
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("native_name")]
        public string NativeName { get; set; }

        [JsonProperty("sample_interval")]
        public string SampleInterval { get; set; }

        [JsonProperty("sample_rate")]
        public string SampleRate { get; set; }
    }
}

using Newtonsoft.Json;
using System;

namespace TrakHound.Api.v2.Data
{
    public class Agent
    {
        [JsonProperty("timestamp")]
        [JsonConverter(typeof(UnixTimeJsonConverter))]
        public DateTime Timestamp { get; set; }

        [JsonProperty("instance_id")]
        public long InstanceId { get; set; }

        [JsonProperty("sender")]
        public string Sender { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("buffer_size")]
        public long BufferSize { get; set; }

        [JsonProperty("test_indicator")]
        public string TestIndicator { get; set; }
    }
}


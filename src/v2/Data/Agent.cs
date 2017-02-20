// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using System;

namespace TrakHound.Api.v2.Data
{
    public class Agent
    {
        [JsonProperty("timestamp")]
        [JsonConverter(typeof(Json.UnixTimeConverter))]
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


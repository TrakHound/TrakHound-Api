﻿// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using System;

namespace TrakHound.Api.v2.Data
{
    public class Sample
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        [JsonConverter(typeof(Json.UnixTimeConverter))]
        public DateTime Timestamp { get; set; }

        [JsonProperty("agent_instance_id")]
        public long AgentInstanceId { get; set; }

        [JsonProperty("sequence")]
        public long Sequence { get; set; }

        [JsonProperty("cdata")]
        public string CDATA { get; set; }

        [JsonProperty("condition")]
        public string Condition { get; set; }
    }

    public class ReturnedSample : Sample
    {
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}


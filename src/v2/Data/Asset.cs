// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using System;

namespace TrakHound.Api.v2.Data
{
    public class Asset
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        [JsonConverter(typeof(Json.UnixTimeConverter))]
        public DateTime Timestamp { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("xml")]
        public string Xml { get; set; }
    }

    public class AssetDefinition : Asset
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("agent_instance_id")]
        public long AgentInstanceId { get; set; }
    }
}

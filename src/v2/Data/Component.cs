// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

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
        public double SampleInterval { get; set; }

        [JsonProperty("sample_rate")]
        public double SampleRate { get; set; }
    }
}

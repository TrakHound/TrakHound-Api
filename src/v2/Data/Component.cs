// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using System.Collections.Generic;

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

    public class ComponentDefinition : Component
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("agent_instance_id")]
        public long AgentInstanceId { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("parent_type")]
        public string ParentType { get; set; }
    }

    public class ComponentModel : Component
    {
        [JsonProperty("components", Order = 5)]
        public List<ComponentModel> Components { get; set; }

        [JsonProperty("data_items", Order = 4)]
        public List<DataItem> DataItems { get; set; }
    }

}

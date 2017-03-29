// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;

namespace TrakHound.Api.v2.Data
{
    public class DataItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("sub_type")]
        public string SubType { get; set; }

        [JsonProperty("statistic")]
        public string Statistic { get; set; }

        [JsonProperty("units")]
        public string Units { get; set; }

        [JsonProperty("native_units")]
        public string NativeUnits { get; set; }

        [JsonProperty("native_scale")]
        public string NativeScale { get; set; }

        [JsonProperty("coordinate_system")]
        public string CoordinateSystem { get; set; }

        [JsonProperty("sample_rate")]
        public double SampleRate { get; set; }

        [JsonProperty("representation")]
        public string Representation { get; set; }

        [JsonProperty("significant_digits")]
        public int SignificantDigits { get; set; }
    }

    public class DataItemDefinition : DataItem
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("agent_instance_id")]
        public long AgentInstanceId { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }
    }
}

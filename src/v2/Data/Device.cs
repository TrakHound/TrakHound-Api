// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace TrakHound.Api.v2.Data
{
    public class Device
    {
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

        [JsonProperty("iso_841_class")]
        public string Iso841Class { get; set; }


        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("serial_number")]
        public string SerialNumber { get; set; }

        [JsonProperty("station")]
        public string Station { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class DeviceDefinition : Device
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("agent_instance_id")]
        public long AgentInstanceId { get; set; }
    }

    public class DeviceModel : Device
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("connection")]
        public Connection Connection { get; set; }

        [JsonProperty("agent")]
        public Agent Agent { get; set; }

        [JsonProperty("components", Order = 5)]
        public List<ComponentModel> Components { get; set; }

        [JsonProperty("data_items", Order = 4)]
        public List<DataItem> DataItems { get; set; }

        public List<DataItem> GetDataItems()
        {
            var dataItems = new List<DataItem>();
            if (DataItems != null) dataItems.AddRange(DataItems);
            if (Components != null)
            {
                foreach (var subcomponent in Components) dataItems.AddRange(GetDataItems(subcomponent));
            }

            return dataItems;
        }

        public List<DataItem> GetDataItems(ComponentModel component)
        {
            var dataItems = new List<DataItem>();
            if (component.DataItems != null) dataItems.AddRange(component.DataItems);
            if (component.Components != null)
            {
                foreach (var subcomponent in component.Components) dataItems.AddRange(GetDataItems(subcomponent));
            }

            return dataItems;
        }

        public List<ComponentModel> GetComponents()
        {
            var components = new List<ComponentModel>();
            if (Components != null)
            {
                foreach (var component in Components)
                {
                    components.Add(component);
                    components.AddRange(GetComponents(component));
                }
            }

            return components;
        }

        public List<ComponentModel> GetComponents(ComponentModel component)
        {
            var components = new List<ComponentModel>();
            components.Add(component);
            if (component.Components != null)
            {
                foreach (var subcomponent in component.Components) components.AddRange(GetComponents(subcomponent));
            }

            return components;
        }
    }
}

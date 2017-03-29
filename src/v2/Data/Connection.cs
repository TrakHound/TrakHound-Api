// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;

namespace TrakHound.Api.v2.Data
{
    public class Connection
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("physical_address")]
        public string PhysicalAddress { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }
    }

    public class ConnectionDefinition : Connection
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }
    }
}

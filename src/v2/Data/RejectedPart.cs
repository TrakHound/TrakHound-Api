// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using System;

namespace TrakHound.Api.v2.Data
{
    public class RejectedPart
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("part_id")]
        public string PartId { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}

// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using System;

namespace TrakHound.Api.v2.Data
{
    public class Alarm
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("data_item_id")]
        public string DataItemId { get; set; }

        [JsonProperty("condition")]
        public string Condition { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}


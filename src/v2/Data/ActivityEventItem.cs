// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace TrakHound.Api.v2.Data
{
    public class ActivityEventItem
    {
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("value_description")]
        public string ValueDescription { get; set; }

        [JsonProperty("triggers")]
        public List<ActivityEventItemTrigger> Triggers { get; set; }
    }
}

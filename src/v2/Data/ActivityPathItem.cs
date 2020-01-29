// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using System.Collections.Generic;


namespace TrakHound.Api.v2.Data
{
    public class ActivityPathItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("events")]
        public List<ActivityEventItem> Events { get; set; }


        public ActivityPathItem()
        {
            Events = new List<ActivityEventItem>();
        }
    }
}

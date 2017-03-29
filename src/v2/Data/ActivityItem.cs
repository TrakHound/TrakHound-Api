// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using System.Collections.Generic;


namespace TrakHound.Api.v2.Data
{
    public class ActivityItem
    {
        [JsonProperty("events")]
        public List<ActivityEventItem> Events { get; set; }

        [JsonProperty("paths")]
        public List<ActivityPathItem> Paths { get; set; }
    }
}

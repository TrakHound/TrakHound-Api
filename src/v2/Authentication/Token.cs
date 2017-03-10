// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using System;

namespace TrakHound.Api.v2.Authentication
{
    public class Token
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("ttl")]
        public int TTL { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("updated")]
        public DateTime Updated { get; set; }
    }
}

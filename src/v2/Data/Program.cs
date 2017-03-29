// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrakHound.Api.v2.Data
{
    public class Program
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("completed")]
        public bool Completed { get; set; }

        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("stop")]
        public DateTime Stop { get; set; }

        [JsonProperty("duration")]
        public TimeSpan Duration { get; set; }

        [JsonProperty("events")]
        public List<ProgramEvent> Events { get; set; }
    }
}


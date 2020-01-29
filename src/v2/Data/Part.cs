// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using System;

namespace TrakHound.Api.v2.Data
{
    public class Part
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("program_name")]
        public string ProgramName { get; set; }

        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("stop")]
        public DateTime Stop { get; set; }

        [JsonProperty("duration")]
        public TimeSpan Duration { get; set; }
    }
}

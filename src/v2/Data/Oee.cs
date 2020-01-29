// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrakHound.Api.v2.Data
{
    public class Oee
    {
        [JsonProperty("oee")]
        public double Value { get; set; }

        [JsonProperty("from")]
        public DateTime From { get; set; }

        [JsonProperty("to")]
        public DateTime To { get; set; }

        [JsonProperty("availability")]
        public Availability Availability { get; set; }

        [JsonProperty("performance")]
        public Performance Performance { get; set; }

        [JsonProperty("quality")]
        public Quality Quality { get; set; }
    }

    public class Availability
    {
        [JsonProperty("operating_time")]
        public double OperatingTime { get; set; }

        [JsonProperty("planned_production_time")]
        public double PlannedProductionTime { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }

        [JsonProperty("events")]
        public List<AvailabilityEvent> Events { get; set; }
    }

    public class AvailabilityEvent
    {
        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("stop")]
        public DateTime Stop { get; set; }

        [JsonProperty("duration")]
        public double Duration { get; set; }
    }

    public class Performance
    {
        [JsonProperty("operating_time")]
        public double OperatingTime { get; set; }

        [JsonProperty("ideal_operating_time")]
        public double IdealOperatingTime { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }

        [JsonProperty("events")]
        public List<PerformanceEvent> Events { get; set; }
    }

    public class PerformanceEvent
    {
        [JsonProperty("feedrate_override")]
        public double FeedrateOverride { get; set; }

        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("stop")]
        public DateTime Stop { get; set; }

        [JsonProperty("total_time")]
        public double TotalTime { get; set; }

        [JsonProperty("operating_time")]
        public double OperatingTime { get; set; }

        [JsonProperty("ideal_operating_time")]
        public double IdealOperatingTime { get; set; }
    }

    public class Quality
    {
        [JsonProperty("total_parts")]
        public int TotalParts { get; set; }

        [JsonProperty("good_parts")]
        public int GoodParts { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }
}

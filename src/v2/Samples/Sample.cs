// Copyright (c) 2017 TrakHound Inc, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using System;

namespace TrakHound.Api.v2
{
    public partial class Samples
    {
        public class Sample
        {
            /// <summary>
            /// The Device that this Sample is for
            /// </summary>
            [JsonProperty("device_id")]
            public string DeviceId { get; set; }

            /// <summary>
            /// The MTConnect DataItem ID for the Sample
            /// </summary>
            [JsonProperty("id")]
            public string Id { get; set; }

            /// <summary>
            /// The Timestamp that is associated with the Sample
            /// </summary>
            [JsonProperty("timestamp")]
            [JsonConverter(typeof(EpochJsonConverter))]
            public DateTime Timestamp { get; set; }

            /// <summary>
            /// The MTConnect Sequence number for this Sample
            /// </summary>
            [JsonProperty("sequence")]
            public long Sequence { get; set; }

            /// <summary>
            /// The first value for the Sample
            /// </summary>
            [JsonProperty("value_1")]
            public string Value1 { get; set; }

            /// <summary>
            /// The second value for the sample (typically used for Condition CDATA)
            /// </summary>
            [JsonProperty("value_2")]
            public string Value2 { get; set; }
        }
    }
}

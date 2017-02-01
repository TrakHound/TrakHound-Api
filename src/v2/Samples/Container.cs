// Copyright (c) 2017 TrakHound Inc, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace TrakHound.Api.v2
{
    public partial class Samples
    {
        /// <summary>
        /// Container of samples for a device
        /// </summary>
        public class Container
        {
            /// <summary>
            /// The ID of the device the Samples are for
            /// </summary>
            [JsonProperty("device_id")]
            public string DeviceId { get; set; }

            /// <summary>
            /// A List of Sample objects for the device
            /// </summary>
            [JsonProperty("samples")]
            public List<Sample> Samples { get; set; }

            public Container()
            {
                Samples = new List<Sample>();
            }
        }
    }
}

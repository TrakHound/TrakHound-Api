// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using System;
using TrakHound.Api.v2.Data;

namespace TrakHound.Api.v2.Streams.Data
{
    /// <summary>
    /// Defines an MTConnect Connection Status
    /// </summary>
    public class StatusData : Status, IStreamData
    {
        /// <summary>
        /// A unique identifier to this particular entry that was received
        /// </summary>
        [JsonIgnore]
        public string EntryId { get; set; }

        /// <summary>
        /// The type of Data
        /// </summary>
        [JsonProperty("stream_data_type")]
        public StreamDataType StreamDataType { get { return StreamDataType.STATUS; } }

        /// <summary>
        /// The ApiKey associated for the TrakHound User Account
        /// </summary>
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        public StatusData()
        {
            EntryId = Guid.NewGuid().ToString();
        }
    }
}

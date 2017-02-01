// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using System;
using TrakHound.Api.v2.Data;

namespace TrakHound.Api.v2.Streams.Data
{
    /// <summary>
    /// Defines an MTConnect Sample used for streaming
    /// </summary>
    public class SampleData : Sample, IStreamData
    {
        /// <summary>
        /// A unique identifier to this particular entry that was received
        /// </summary>
        [JsonIgnore]
        public string EntryId { get; set; }

        private StreamDataType _streamDataType = StreamDataType.ARCHIVED_SAMPLE;
        /// <summary>
        /// The type of Data
        /// </summary>
        [JsonProperty("stream_data_type")]
        public StreamDataType StreamDataType { get { return _streamDataType; } }

        /// <summary>
        /// The ApiKey associated for the TrakHound User Account
        /// </summary>
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        public SampleData()
        {
            EntryId = Guid.NewGuid().ToString();
        }

        public SampleData(StreamDataType type)
        {
            EntryId = Guid.NewGuid().ToString();
            _streamDataType = type;
        }


        public void SetStreamDataType(StreamDataType type)
        {
            _streamDataType = type;
        }
    }
}

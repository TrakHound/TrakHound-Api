// Copyright (c) 2017 TrakHound Inc, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace TrakHound.Api.v2.Streams
{
    /// <summary>
    /// Interface for Data sent using streaming
    /// </summary>
    public interface IStreamData
    {
        /// <summary>
        /// A unique identifier to this particular Sample that was received
        /// </summary>
        string EntryId { get; set; }

        /// <summary>
        /// The type of Data
        /// </summary>
        StreamDataType StreamDataType { get; }

        /// <summary>
        /// The ApiKey associated for the TrakHound User Account
        /// </summary>
        string ApiKey { get; set; }

        /// <summary>
        /// The Device that this Data is for
        /// </summary>
        string DeviceId { get; set; }
    }
}

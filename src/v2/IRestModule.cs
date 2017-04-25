// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.IO;

namespace TrakHound.Api.v2
{
    public interface IRestModule
    {
        /// <summary>
        /// Gets the name of the Module
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Processes the requested Uri and returns whether the request was handled
        /// </summary>
        bool GetResponse(Uri requestUri, Stream stream);

        /// <summary>
        /// Sends data to the requested Uri and returns whether the request was handled
        /// </summary>
        bool SendData(Uri requestUri, Stream stream);

        /// <summary>
        /// Delete data at the requested Uri and returns whether request was handled
        /// </summary>
        bool DeleteData(Uri requestUri);
    }
}

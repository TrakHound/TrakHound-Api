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
        /// Processes the requested Uri and returns the requested data
        /// </summary>
        bool GetResponse(Uri requestUri, Stream stream);
    }
}

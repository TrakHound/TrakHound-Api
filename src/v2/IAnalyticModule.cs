// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Collections.Generic;
using TrakHound.Api.v2.Data;
using TrakHound.Api.v2.Events;

namespace TrakHound.Api.v2
{
    public interface IAnalyticModule
    {
        /// <summary>
        /// Gets the name of the Module
        /// </summary>
        string Name { get; }

        int Interval { get; }

        List<string> RunTimes { get; }

        List<Trigger> Triggers { get; }


        bool Run();

        bool Run(string deviceId);

        bool Run(Sample triggerSample);


        void Close();
    }
}

// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace TrakHound.Api.v2.Data
{
    public class ActivityStatistic
    {
        public string DeviceId { get; set; }

        public int Interval { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public double Total { get; set; }

        public double Active { get; set; }

        public double Idle { get; set; }

        public double Alert { get; set; }
    }
}

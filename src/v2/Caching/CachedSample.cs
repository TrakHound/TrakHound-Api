// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using TrakHound.Api.v2.Data;

namespace TrakHound.Api.v2.Caching
{
    public class CachedSample
    {
        public DateTime Added { get; set; }

        public DateTime LastUsed { get; set; }

        public Sample Sample { get; set; }


        public CachedSample(Sample sample)
        {
            Sample = sample;
            Added = DateTime.UtcNow;
            LastUsed = Added;
        }
    }
}

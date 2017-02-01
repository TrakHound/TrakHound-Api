// Copyright (c) 2017 TrakHound Inc, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Collections.Generic;
using TrakHound.Api.v2.Data;

namespace TrakHound.Api.v2.Events
{
    public interface IEvaluator
    {
        bool Evaluate(List<SampleInfo> samples);
    }
}

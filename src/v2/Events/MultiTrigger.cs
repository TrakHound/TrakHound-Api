// Copyright (c) 2017 TrakHound Inc, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Collections.Generic;
using System.Xml.Serialization;
using TrakHound.Api.v2.Data;

namespace TrakHound.Api.v2.Events
{
    public class MultiTrigger : IEvaluator
    {
        [XmlArray("Triggers")]
        [XmlArrayItem("Trigger")]
        public List<Trigger> Triggers { get; set; }

        public bool Evaluate(List<SampleInfo> samples)
        {
            if (!samples.IsNullOrEmpty() && !Triggers.IsNullOrEmpty())
            {
                foreach (var trigger in Triggers)
                {
                    if (trigger.Evaluate(samples)) return true; 
                }
            }

            return false;
        }
    }
}

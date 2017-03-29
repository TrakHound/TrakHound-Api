// Copyright (c) 2017 TrakHound Inc, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TrakHound.Api.v2.Data;

namespace TrakHound.Api.v2.Events
{
    public class Response : IEvaluator
    {
        /// <summary>
        /// Name of the Response
        /// </summary>
        [XmlAttribute("value")]
        public string Value { get; set; }

        /// <summary>
        /// Description of the Response
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }

        /// <summary>
        /// List of Triggers and Multitriggers that perform logic for the Response
        /// </summary>
        [XmlArray("Triggers")]
        [XmlArrayItem("Trigger", typeof(Trigger))]
        [XmlArrayItem("MultiTrigger", typeof(MultiTrigger))]
        public List<object> Triggers { get; set; }

        [XmlIgnore]
        public DateTime Timestamp { get; set; }

        [XmlIgnore]
        public object UserObject { get; set; }


        public bool Evaluate(List<SampleInfo> samples)
        {
            if (!samples.IsNullOrEmpty())
            {
                // Test Triggers
                if (!Triggers.IsNullOrEmpty())
                {
                    foreach (var obj in Triggers)
                    {
                        var trigger = obj as IEvaluator;
                        if (trigger != null)
                        {
                            if (!trigger.Evaluate(samples)) return false;
                        }
                        else return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}

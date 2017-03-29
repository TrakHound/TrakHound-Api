// Copyright (c) 2017 TrakHound Inc, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Collections.Generic;
using System.Xml.Serialization;
using TrakHound.Api.v2.Data;
using System.Linq;

namespace TrakHound.Api.v2.Events
{
    public class Event
    {
        /// <summary>
        /// Name of the Event
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Description of the Event
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }

        /// <summary>
        /// List of possible Responses
        /// </summary>
        [XmlArray("Responses")]
        [XmlArrayItem("Response")]
        public List<Response> Responses { get; set; }


        public Response Evaluate(List<SampleInfo> samples)
        {
            foreach (var response in Responses)
            {
                if (response.Evaluate(samples))
                {
                    // Get Latest Trigger Sample Timestamp
                    foreach (var trigger in response.Triggers.OfType<Trigger>())
                    {
                        var matches = samples.FindAll(o => trigger.CheckFilter(o));
                        if (matches != null)
                        {
                            foreach (var match in matches)
                            {
                                if (match.Timestamp > response.Timestamp) response.Timestamp = match.Timestamp;
                            }
                        }
                    }

                    // Get Latest Trigger Sample Timestamp
                    foreach (var multiTrigger in response.Triggers.OfType<MultiTrigger>())
                    {
                        foreach (var trigger in multiTrigger.Triggers)
                        {
                            var matches = samples.FindAll(o => trigger.CheckFilter(o));
                            if (matches != null)
                            {
                                foreach (var match in matches)
                                {
                                    if (match.Timestamp > response.Timestamp) response.Timestamp = match.Timestamp;
                                }
                            }
                        }                 
                    }

                    return response;
                }
            }

            return null;
        }
    }
}

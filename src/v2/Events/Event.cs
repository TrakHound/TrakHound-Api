// Copyright (c) 2019 TrakHound Inc, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TrakHound.Api.v2.Data;

namespace TrakHound.Api.v2.Events
{
    public class Event
    {
        /// <summary>
        /// Device ID for the Event
        /// </summary>
        [XmlAttribute("deviceId")]
        public string DeviceId { get; set; }

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

        public static string[] GetIds(Event e, List<DataItemDefinition> dataItems, List<ComponentDefinition> components)
        {
            var ids = new List<string>();

            foreach (var response in e.Responses)
            {
                foreach (var trigger in response.Triggers.OfType<Trigger>())
                {
                    foreach (var id in GetFilterIds(trigger.Filter, dataItems, components))
                    {
                        if (!ids.Exists(o => o == id)) ids.Add(id);
                    }
                }

                foreach (var multiTrigger in response.Triggers.OfType<MultiTrigger>())
                {
                    foreach (var trigger in multiTrigger.Triggers)
                    {
                        foreach (var id in GetFilterIds(trigger.Filter, dataItems, components))
                        {
                            if (!ids.Exists(o => o == id)) ids.Add(id);
                        }
                    }
                }
            }

            return ids.ToArray();
        }

        private static string[] GetFilterIds(string filter, List<DataItemDefinition> dataItems, List<ComponentDefinition> components)
        {
            var ids = new List<string>();

            foreach (var dataItem in dataItems)
            {
                var dataFilter = new DataFilter(filter, dataItem, components);
                if (dataFilter.IsMatch() && !ids.Exists(o => o == dataItem.Id))
                {
                    ids.Add(dataItem.Id);
                }
            }

            return ids.ToArray();
        }
    }
}

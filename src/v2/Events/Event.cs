// Copyright (c) 2017 TrakHound Inc, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Collections.Generic;
using System.Xml.Serialization;
using TrakHound.Api.v2.Data;

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
                if (response.Evaluate(samples)) return response;
            }

            return null;
        }
    }
}

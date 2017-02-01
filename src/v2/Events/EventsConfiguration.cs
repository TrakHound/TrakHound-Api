// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace TrakHound.Api.v2.Events
{
    [XmlRoot("EventsConfiguration")]
    public class EventsConfiguration
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        [XmlIgnore]
        public const string FILENAME = "events.config";

        /// <summary>
        /// List of Events
        /// </summary>
        [XmlArray("Events")]
        [XmlArrayItem("Event")]
        public List<Event> Events { get; set; }

        public static EventsConfiguration Get(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    var serializer = new XmlSerializer(typeof(EventsConfiguration));
                    using (var fileReader = new FileStream(path, FileMode.Open))
                    using (var xmlReader = XmlReader.Create(fileReader))
                    {
                        return (EventsConfiguration)serializer.Deserialize(xmlReader);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

            return null;
        }

    }
}

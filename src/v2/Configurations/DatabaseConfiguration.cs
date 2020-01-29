// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.

using Newtonsoft.Json;
using System.Xml;
using System.Xml.Serialization;

namespace TrakHound.Api.v2.Configurations
{
    public class DatabaseConfiguration
    {
        [XmlAttribute("type")]
        [JsonProperty("type")]
        public string Type { get; set; }

        [XmlAttribute("server")]
        [JsonProperty("server")]
        public string Server { get; set; }

        [XmlAttribute("port")]
        [JsonProperty("port")]
        public int Port { get; set; }

        [XmlAttribute("user")]
        [JsonProperty("user")]
        public string User { get; set; }

        [XmlAttribute("password")]
        [JsonProperty("password")]
        public string Password { get; set; }

        [XmlAttribute("databaseName")]
        [JsonProperty("databaseName")]
        public string DatabaseName { get; set; }

        [XmlAttribute("connectionTimeout")]
        [JsonProperty("connectionTimeout")]
        public int ConnectionTimeout { get; set; }

        [XmlAttribute("connectionRetryInterval")]
        [JsonProperty("connectionRetryInterval")]
        public int ConnectionRetryInterval { get; set; }

        [XmlAttribute("queryTimeout")]
        [JsonProperty("queryTimeout")]
        public int QueryTimeout { get; set; }


        public DatabaseConfiguration()
        {
            ConnectionTimeout = 5000;
            ConnectionRetryInterval = 5000;
            QueryTimeout = 60000;
        }
    }
}

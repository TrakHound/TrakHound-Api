// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using RestSharp;
using System.Collections.Generic;
using System.Linq;
using TrakHound.Api.v2.Data;


namespace TrakHound.Api.v2.Requests
{
    public static class Connections
    {
        /// <summary>
        /// Request a list of connections from the AnalyticsServer
        /// </summary>
        public static IEnumerable<ConnectionDefinition> Get(string baseUrl)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest("connections", Method.GET);

            var response = client.Execute(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = response.Content;
                if (!string.IsNullOrEmpty(json))
                {
                    var obj = Json.Convert.FromJson<IEnumerable<ConnectionDefinition>>(json);
                    if (obj != null) return obj;
                }
            }

            return Enumerable.Empty<ConnectionDefinition>();
        }

        /// <summary>
        /// Request a connection for a single device
        /// </summary>
        public static Connection Get(string baseUrl, string deviceId)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(deviceId + "/connection", Method.GET);

            var response = client.Execute(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = response.Content;
                if (!string.IsNullOrEmpty(json))
                {
                    var obj = Json.Convert.FromJson<Connection>(json);
                    if (obj != null) return obj;
                }
            }

            return null;
        }

    }
}

using System;
// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using RestSharp;
using System.Collections.Generic;

namespace TrakHound.Api.v2.Requests
{
    public static class Parts
    {
        /// <summary>
        /// Request Alarms for a single device
        /// </summary>
        public static List<Data.Part> Get(string baseUrl, string deviceId, DateTime from, DateTime to, string accessToken = null)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(deviceId + "/parts", Method.GET);
            if (from > DateTime.MinValue) request.AddQueryParameter("from", from.ToString("o"));
            if (to > DateTime.MinValue) request.AddQueryParameter("to", to.ToString("o"));
            if (!string.IsNullOrEmpty(accessToken)) request.AddQueryParameter("access_token", accessToken);

            var response = client.Execute(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = response.Content;
                if (!string.IsNullOrEmpty(json))
                {
                    var obj = Json.Convert.FromJson<List<Data.Part>>(json);
                    if (obj != null) return obj;
                }
            }

            return null;
        }

        #region "Overloads"

        public static List<Data.Part> Get(string baseUrl, string deviceId, DateTime from)
        {
            return Get(baseUrl, deviceId, from, DateTime.MinValue);
        }

        #endregion
    }
}

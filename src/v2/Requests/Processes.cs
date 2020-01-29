// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using RestSharp;
using System;
using System.Collections.Generic;

namespace TrakHound.Api.v2.Requests
{
    public class Processes
    {
        public static List<Data.Process> Get(string baseUrl, string deviceId, string processType, DateTime from, DateTime to, string accessToken)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(deviceId + "/processes", Method.GET);
            if (!string.IsNullOrEmpty(processType)) request.AddQueryParameter("type", processType);
            if (from > DateTime.MinValue) request.AddQueryParameter("from", from.ToString("o"));
            if (to > DateTime.MinValue) request.AddQueryParameter("to", to.ToString("o"));
            if (!string.IsNullOrEmpty(accessToken)) request.AddQueryParameter("access_token", accessToken);

            var response = client.Execute(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = response.Content;
                if (!string.IsNullOrEmpty(json))
                {
                    var obj = Json.Convert.FromJson<List<Data.Process>>(json);
                    if (obj != null) return obj;
                }
            }

            return null;
        }
    }
}

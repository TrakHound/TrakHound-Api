// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using RestSharp;
using System.Collections.Generic;
using TrakHound.Api.v2.Data;

namespace TrakHound.Api.v2.Requests
{
    public static class Model
    {
        /// <summary>
        /// Request a model for a single device
        /// </summary>
        public static DeviceModel Get(string baseUrl, string deviceId, string accessToken, bool descriptionOnly = false)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(deviceId + "/model", Method.GET);
            if (!string.IsNullOrEmpty(accessToken)) request.AddQueryParameter("access_token", accessToken);
            if (descriptionOnly) request.AddQueryParameter("descriptionOnly", "true");

            var response = client.Execute(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = response.Content;
                if (!string.IsNullOrEmpty(json))
                {
                    var obj = Json.Convert.FromJson<DeviceModel>(json);
                    if (obj != null)
                    {
                        obj.DeviceId = deviceId;
                        return obj;
                    }
                }
            }

            return null;
        }

        public static List<DeviceModel> Get(string baseUrl, string accessToken, bool descriptionOnly = false)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest("/models", Method.GET);
            if (!string.IsNullOrEmpty(accessToken)) request.AddQueryParameter("access_token", accessToken);
            if (descriptionOnly) request.AddQueryParameter("descriptionOnly", "true");

            var response = client.Execute(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = response.Content;
                if (!string.IsNullOrEmpty(json))
                {
                    var objs = Json.Convert.FromJson<List<DeviceModel>>(json);
                    if (objs != null) return objs;
                }
            }

            return null;
        }
    }
}

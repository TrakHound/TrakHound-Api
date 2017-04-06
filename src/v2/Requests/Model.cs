// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using RestSharp;
using TrakHound.Api.v2.Data;

namespace TrakHound.Api.v2.Requests
{
    public static class Model
    {
        /// <summary>
        /// Request a model for a single device
        /// </summary>
        public static DeviceModel Get(string baseUrl, string deviceId, string accessToken)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(deviceId + "/model", Method.GET);
            if (!string.IsNullOrEmpty(accessToken)) request.AddQueryParameter("access_token", accessToken);

            var response = client.Execute(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = response.Content;
                if (!string.IsNullOrEmpty(json))
                {
                    var obj = Json.Convert.FromJson<DeviceModel>(json);
                    if (obj != null) return obj;
                }
            }

            return null;
        }
    }
}

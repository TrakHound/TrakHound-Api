// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using RestSharp;

namespace TrakHound.Api.v2.Devices
{
    public class Device
    {
        public string Id { get; set; }

        public string UserId { get; set; }


        public static bool Exists(string apiKey, string deviceId, string url)
        {
            if (!string.IsNullOrEmpty(apiKey))
            {
                var client = new RestClient(url);

                var request = new RestRequest("api/devices", Method.GET);
                request.AddParameter("api_key", apiKey);
                if (!string.IsNullOrEmpty(deviceId)) request.AddParameter("device_id", deviceId);

                var response = client.Execute(request);
                if (response != null) return response.StatusCode == System.Net.HttpStatusCode.OK;
            }

            return false;
        }

        public static bool Create(string apiKey, string deviceId, string url)
        {
            if (!string.IsNullOrEmpty(apiKey) && !string.IsNullOrEmpty(deviceId))
            {
                var client = new RestClient(url);

                var request = new RestRequest();
                request.Method = Method.POST;
                request.AddParameter("api_key", apiKey);
                request.AddParameter("device_id", deviceId);

                var response = client.Execute(request);
                if (response != null) return response.StatusCode == System.Net.HttpStatusCode.OK;
            }

            return false;
        }
    }
}

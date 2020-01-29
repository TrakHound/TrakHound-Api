﻿// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using RestSharp;

namespace TrakHound.Api.v2.Requests
{
    public static class Status
    {
        /// <summary>
        /// Request a status for a single device
        /// </summary>
        public static Data.Status Get(string baseUrl, string deviceId, string accessToken)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(deviceId + "/status", Method.GET);
            if (!string.IsNullOrEmpty(accessToken)) request.AddQueryParameter("access_token", accessToken);

            var response = client.Execute(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = response.Content;
                if (!string.IsNullOrEmpty(json))
                {
                    var obj = Json.Convert.FromJson<Data.Status>(json);
                    if (obj != null) return obj;
                }
            }

            return null;
        }

        //public static Data.ReturnedStatus Get(string baseUrl, string deviceId, string accessToken)
        //{
        //    var client = new RestClient(baseUrl);
        //    var request = new RestRequest(deviceId + "/status", Method.GET);
        //    if (!string.IsNullOrEmpty(accessToken)) request.AddQueryParameter("access_token", accessToken);

        //    var response = client.Execute(request);
        //    if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        var json = response.Content;
        //        if (!string.IsNullOrEmpty(json))
        //        {
        //            var obj = Json.Convert.FromJson<Data.ReturnedStatus>(json);
        //            if (obj != null) return obj;
        //        }
        //    }

        //    return null;
        //}
    }
}

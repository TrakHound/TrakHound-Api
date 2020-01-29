// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using RestSharp;
using System;
using System.Collections.Generic;

namespace TrakHound.Api.v2.Requests
{
    public class Activity
    {
        /// <summary>
        /// Request Alarms for a single device
        /// </summary>
        public static Data.ActivityItem Get(string baseUrl, string deviceId, DateTime from, DateTime to, int count, string eventName, string accessToken)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(deviceId + "/activity", Method.GET);
            if (from > DateTime.MinValue) request.AddQueryParameter("from", from.ToString("o"));
            if (to > DateTime.MinValue) request.AddQueryParameter("to", to.ToString("o"));
            if (count > 0) request.AddQueryParameter("count", count.ToString());
            if (!string.IsNullOrEmpty(accessToken)) request.AddQueryParameter("access_token", accessToken);
            if (!string.IsNullOrEmpty(eventName)) request.AddQueryParameter("name", eventName);

            var response = client.Execute(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = response.Content;
                if (!string.IsNullOrEmpty(json))
                {
                    var obj = Json.Convert.FromJson<Data.ActivityItem>(json);
                    if (obj != null) return obj;
                }
            }

            return null;
        }

        #region "Overloads"

        public static Data.ActivityItem Get(string baseUrl, string deviceId, DateTime from, string eventName = null, string accessToken = null)
        {
            return Get(baseUrl, deviceId, from, DateTime.MinValue, 0, eventName, accessToken);
        }

        public static Data.ActivityItem Get(string baseUrl, string deviceId, int count, string eventName = null, string accessToken = null)
        {
            return Get(baseUrl, deviceId, DateTime.MinValue, DateTime.MinValue, count, eventName, accessToken);
        }

        public static Data.ActivityItem Get(string baseUrl, string deviceId, DateTime from, int count, string eventName = null, string accessToken = null)
        {
            return Get(baseUrl, deviceId, from, DateTime.MinValue, count, eventName, accessToken);
        }

        public static Data.ActivityItem Get(string baseUrl, string deviceId, string eventName = null, string accessToken = null)
        {
            return Get(baseUrl, deviceId, DateTime.MinValue, DateTime.MinValue, 0, eventName, accessToken);
        }

        #endregion

        public static List<Data.ActivityStatistic> GetStatistics(string baseUrl, string deviceId, DateTime from, DateTime to, string accessToken)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(deviceId + "/activity/statistics", Method.GET);
            if (from > DateTime.MinValue) request.AddQueryParameter("from", from.ToString("o"));
            if (to > DateTime.MinValue) request.AddQueryParameter("to", to.ToString("o"));
            if (!string.IsNullOrEmpty(accessToken)) request.AddQueryParameter("access_token", accessToken);

            var response = client.Execute(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = response.Content;
                if (!string.IsNullOrEmpty(json))
                {
                    var obj = Json.Convert.FromJson<List<Data.ActivityStatistic>>(json);
                    if (obj != null) return obj;
                }
            }

            return null;
        }


        public class Stream
        {
            private Requests.Stream stream;

            public string BaseUrl { get; set; }
            public string DeviceId { get; set; }
            public int Interval { get; set; }
            public string AccessToken { get; set; }

            public delegate void ActivityHandler(Data.ActivityItem activity);
            public event ActivityHandler ActivityReceived;

            public Stream(string baseUrl, string deviceId, int interval, string accessToken = null)
            {
                BaseUrl = baseUrl;
                DeviceId = deviceId;
                Interval = interval;
                AccessToken = accessToken;
            }

            public void Start()
            {
                var uri = new Uri(BaseUrl);
                var query = "?interval=" + Interval;
                if (!string.IsNullOrEmpty(AccessToken)) query += "&access_token=" + AccessToken;
                uri = new Uri(uri, DeviceId + "/activity" + query);

                stream = new Requests.Stream(uri.ToString(), "{", "\r\n");
                stream.GroupReceived += Stream_GroupReceived;
                stream.Start();
            }

            public void Stop()
            {
                if (stream != null) stream.Stop();
            }

            private void Stream_GroupReceived(string json)
            {
                var obj = Json.Convert.FromJson<Data.ActivityItem>(json);
                if (obj != null) ActivityReceived?.Invoke(obj);
            }
        }
    }
}

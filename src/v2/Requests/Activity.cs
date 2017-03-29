﻿// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using RestSharp;
using System;


namespace TrakHound.Api.v2.Requests
{
    public class Activity
    {
        /// <summary>
        /// Request Alarms for a single device
        /// </summary>
        public static Data.ActivityItem Get(string baseUrl, string deviceId, DateTime from, DateTime to, int count)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(deviceId + "/activity", Method.GET);
            if (from > DateTime.MinValue) request.AddQueryParameter("from", from.ToString("o"));
            if (to > DateTime.MinValue) request.AddQueryParameter("to", to.ToString("o"));
            if (count > 0) request.AddQueryParameter("count", count.ToString());

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

        public static Data.ActivityItem Get(string baseUrl, string deviceId, DateTime from)
        {
            return Get(baseUrl, deviceId, from, DateTime.MinValue, 0);
        }

        public static Data.ActivityItem Get(string baseUrl, string deviceId, int count)
        {
            return Get(baseUrl, deviceId, DateTime.MinValue, DateTime.MinValue, count);
        }

        public static Data.ActivityItem Get(string baseUrl, string deviceId, DateTime from, int count)
        {
            return Get(baseUrl, deviceId, from, DateTime.MinValue, count);
        }

        public static Data.ActivityItem Get(string baseUrl, string deviceId)
        {
            return Get(baseUrl, deviceId, DateTime.MinValue, DateTime.MinValue, 0);
        }

        #endregion


        public class Stream
        {
            private Requests.Stream stream;

            public string BaseUrl { get; set; }
            public int Interval { get; set; }

            public delegate void ActivityHandler(Data.ActivityItem activity);
            public event ActivityHandler ActivityReceived;

            public Stream(string baseUrl, string deviceId, int interval)
            {
                var uri = new Uri(baseUrl);
                uri = new Uri(uri, deviceId + "/activity?interval=" + interval);

                stream = new Requests.Stream(uri.ToString(), "{", "\r\n");
                stream.GroupReceived += Stream_GroupReceived;
            }

            public void Start()
            {
                if (stream != null) stream.Start();
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

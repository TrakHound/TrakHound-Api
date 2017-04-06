// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using RestSharp;
using System;
using System.Collections.Generic;

namespace TrakHound.Api.v2.Requests
{
    public static class Alarms
    {
        /// <summary>
        /// Request Alarms for a single device
        /// </summary>
        public static List<Data.Alarm> Get(string baseUrl, string deviceId, DateTime from, DateTime to, int count, string accessToken)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(deviceId + "/alarms", Method.GET);
            if (from > DateTime.MinValue) request.AddQueryParameter("from", from.ToString("o"));
            if (to > DateTime.MinValue) request.AddQueryParameter("to", to.ToString("o"));
            if (count > 0) request.AddQueryParameter("count", count.ToString());
            if (!string.IsNullOrEmpty(accessToken)) request.AddQueryParameter("access_token", accessToken);

            var response = client.Execute(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = response.Content;
                if (!string.IsNullOrEmpty(json))
                {
                    var obj = Json.Convert.FromJson<List<Data.Alarm>>(json);
                    if (obj != null) return obj;
                }
            }

            return null;
        }

        #region "Overloads"

        public static List<Data.Alarm> Get(string baseUrl, string deviceId, DateTime from, string accessToken = null)
        {
            return Get(baseUrl, deviceId, from, DateTime.MinValue, 0, accessToken);
        }

        public static List<Data.Alarm> Get(string baseUrl, string deviceId, int count, string accessToken = null)
        {
            return Get(baseUrl, deviceId, DateTime.MinValue, DateTime.MinValue, count, accessToken);
        }

        public static List<Data.Alarm> Get(string baseUrl, string deviceId, DateTime from, int count, string accessToken = null)
        {
            return Get(baseUrl, deviceId, from, DateTime.MinValue, count, accessToken);
        }

        #endregion


        public class Stream
        {
            private Requests.Stream stream;

            public string BaseUrl { get; set; }
            public string DeviceId { get; set; }
            public int Interval { get; set; }
            public string AccessToken { get; set; }

            public delegate void AlarmHandler(Data.Alarm alarm);
            public event AlarmHandler AlarmReceived;

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
                uri = new Uri(uri, DeviceId + "/alarms" + query);

                stream = new Requests.Stream(uri.ToString(), "[", "\r\n");
                stream.GroupReceived += Stream_GroupReceived;
                stream.Start();
            }

            public void Stop()
            {
                if (stream != null) stream.Stop();
            }

            private void Stream_GroupReceived(string json)
            {
                var obj = Json.Convert.FromJson<List<Data.Alarm>>(json);
                if (obj != null)
                {
                    foreach (var alarm in obj)
                    {
                        AlarmReceived?.Invoke(alarm);
                    }
                }
            }
        }
    }
}

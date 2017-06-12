// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using RestSharp;
using System;
using System.Collections.Generic;

namespace TrakHound.Api.v2.Requests
{
    public class Oee
    {
        /// <summary>
        /// Request OEE Data for a single device
        /// </summary>
        public static List<Data.Oee> Get(string baseUrl, string deviceId, DateTime from, DateTime to, int increment, string accessToken)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(deviceId + "/oee", Method.GET);
            if (from > DateTime.MinValue) request.AddQueryParameter("from", from.ToString("o"));
            if (to > DateTime.MinValue) request.AddQueryParameter("to", to.ToString("o"));
            if (increment > 0) request.AddQueryParameter("increment", increment.ToString());
            if (!string.IsNullOrEmpty(accessToken)) request.AddQueryParameter("access_token", accessToken);

            var response = client.Execute(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = response.Content;
                if (!string.IsNullOrEmpty(json))
                {
                    var obj = Json.Convert.FromJson<List<Data.Oee>>(json);
                    if (obj != null) return obj;
                }
            }

            return null;
        }

        #region "Overloads"

        public static List<Data.Oee> Get(string baseUrl, string deviceId, DateTime from, string accessToken = null)
        {
            return Get(baseUrl, deviceId, from, DateTime.MinValue, 0, accessToken);
        }

        public static List<Data.Oee> Get(string baseUrl, string deviceId, DateTime from, DateTime to, string accessToken = null)
        {
            return Get(baseUrl, deviceId, from, to, 0, accessToken);
        }

        #endregion


        public class Stream
        {
            private Requests.Stream stream;

            public string BaseUrl { get; set; }
            public string DeviceId { get; set; }
            public DateTime From { get; set; }
            public DateTime To { get; set; }
            public int Interval { get; set; }
            public int Increment { get; set; }
            public string AccessToken { get; set; }

            public delegate void OeeHandler(List<Data.Oee> oee);
            public event OeeHandler OeeReceived;

            public Stream(string baseUrl, string deviceId, DateTime from, DateTime to, int interval, int increment, string accessToken = null)
            {
                BaseUrl = baseUrl;
                DeviceId = deviceId;
                From = from;
                To = to;
                Interval = interval;
                Increment = increment;
                AccessToken = accessToken;
            }

            public void Start()
            {
                var uri = new Uri(BaseUrl);
                var query = "?interval=" + Interval;
                if (!string.IsNullOrEmpty(AccessToken)) query += "&access_token=" + AccessToken;
                if (From > DateTime.MinValue) query += "&from=" + From.ToString("o");
                if (To > DateTime.MinValue) query += "&to=" + To.ToString("o");
                if (Increment > 0) query += "&increment=" + Increment;
                uri = new Uri(uri, DeviceId + "/oee" + query);

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
                var obj = Json.Convert.FromJson<List<Data.Oee>>(json);
                if (obj != null)
                {
                    OeeReceived?.Invoke(obj);
                }
            }
        }
    }
}

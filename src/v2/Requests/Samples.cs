// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using RestSharp;
using System;
using System.Collections.Generic;
using System.Web;

namespace TrakHound.Api.v2.Requests
{
    public static class Samples
    {
        /// <summary>
        /// Request Samples for a single device
        /// </summary>
        public static List<Data.Sample> Get(string baseUrl, string deviceId, string[] dataItems, DateTime from, DateTime to, int count, string accessToken)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(deviceId + "/samples", Method.GET);
            if (from > DateTime.MinValue) request.AddQueryParameter("from", from.ToString("o"));
            if (to > DateTime.MinValue) request.AddQueryParameter("to", to.ToString("o"));
            if (count > 0) request.AddQueryParameter("count", count.ToString());
            if (!string.IsNullOrEmpty(accessToken)) request.AddQueryParameter("access_token", accessToken);

            if (!dataItems.IsNullOrEmpty())
            {
                var dataItemJson = Json.Convert.ToJson(dataItems);
                request.AddQueryParameter("data_items", dataItemJson);
            }

            var response = client.Execute(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = response.Content;
                if (!string.IsNullOrEmpty(json))
                {
                    var samples = new List<Data.Sample>();

                    var lines = json.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        var obj = Json.Convert.FromJson<Data.ReturnedSample>(line);
                        if (obj != null)
                        {
                            var sample = new Data.Sample();
                            sample.AgentInstanceId = obj.AgentInstanceId;
                            sample.CDATA = obj.CDATA;
                            sample.Condition = obj.Condition;
                            sample.DeviceId = obj.DeviceId;
                            sample.Id = obj.Id;
                            sample.Sequence = obj.Sequence;
                            sample.Timestamp = obj.Timestamp;
                            samples.Add(sample);
                        }
                    }

                    return samples;
                }
            }

            return null;
        }
    

        #region "Overloads"

        public static List<Data.Sample> Get(string baseUrl, string deviceId, string accessToken = null)
        {
            return Get(baseUrl, deviceId, null, DateTime.MinValue, DateTime.MinValue, 0, accessToken);
        }

        public static List<Data.Sample> Get(string baseUrl, string deviceId, DateTime from, string accessToken = null)
        {
            return Get(baseUrl, deviceId, null, from, DateTime.MinValue, 0, accessToken);
        }

        public static List<Data.Sample> Get(string baseUrl, string deviceId, int count, string accessToken = null)
        {
            return Get(baseUrl, deviceId, null, DateTime.MinValue, DateTime.MinValue, count, accessToken);
        }

        public static List<Data.Sample> Get(string baseUrl, string deviceId, DateTime from, int count, string accessToken = null)
        {
            return Get(baseUrl, deviceId, null, from, DateTime.MinValue, count, accessToken);
        }

        #endregion


        public class Stream
        {
            private Requests.Stream stream;

            public string BaseUrl { get; set; }
            public string DeviceId { get; set; }
            public int Interval { get; set; }
            public string AccessToken { get; set; }

            private string[] _dataItemids;
            public string[] DataItemIds
            {
                get { return _dataItemids; }
                set
                {
                    _dataItemids = value;

                    if (stream != null)
                    {
                        Stop();
                        Start();
                    }
                }
            }

            public delegate void SampleHandler(Data.Sample sample);
            public event SampleHandler SampleReceived;

            public Stream(string baseUrl, string deviceId, int interval, string[] dataItemIds = null, string accessToken = null)
            {
                BaseUrl = baseUrl;
                DeviceId = deviceId;
                Interval = interval;
                DataItemIds = dataItemIds;
                AccessToken = accessToken;
            }

            public void Start()
            {
                var uri = new Uri(BaseUrl);
                var query = "?interval=" + Interval;
                if (!string.IsNullOrEmpty(AccessToken)) query += "&access_token=" + AccessToken;
                if (!DataItemIds.IsNullOrEmpty()) query += "&data_items=" + HttpUtility.UrlEncode(Json.Convert.ToJson(DataItemIds));
                uri = new Uri(uri, DeviceId + "/samples" + query);

                stream = new Requests.Stream(uri.ToString(), "{", "}");
                stream.GroupReceived += Stream_GroupReceived;
                stream.Start();
            }

            public void Stop()
            {
                if (stream != null) stream.Stop();
            }

            private void Stream_GroupReceived(string json)
            {
                var obj = Json.Convert.FromJson<Data.ReturnedSample>(json);
                if (obj != null)
                {
                    var sample = new Data.Sample();
                    sample.AgentInstanceId = obj.AgentInstanceId;
                    sample.CDATA = obj.CDATA;
                    sample.Condition = obj.Condition;
                    sample.DeviceId = obj.DeviceId;
                    sample.Id = obj.Id;
                    sample.Sequence = obj.Sequence;
                    sample.Timestamp = obj.Timestamp;
                    SampleReceived?.Invoke(sample);
                }
            }
        }
    }
}

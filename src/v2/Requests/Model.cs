using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrakHound.Api.v2.Requests
{
    public static class Model
    {
        /// <summary>
        /// Request a model for a single device
        /// </summary>
        //public static Data.Model Get(string baseUrl, string deviceId)
        //{
        //    var client = new RestClient(baseUrl);
        //    var request = new RestRequest(deviceId + "/status", Method.GET);

        //    var response = client.Execute(request);
        //    if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        var json = response.Content;
        //        if (!string.IsNullOrEmpty(json))
        //        {
        //            var obj = Json.Convert.FromJson<Data.Status>(json);
        //            if (obj != null) return obj;
        //        }
        //    }

        //    return null;
        //}
    }
}

﻿// Copyright (c) 2017 TrakHound Inc, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.IO;
using System.Net;

namespace TrakHound.Api.v2
{
    public class Requests
    {
        #region "Read"

        public static T Read<T>(string resource, NetworkCredential credential) { return Read<T>(resource, credential, null); }

        public static T Read<T>(string resource, string accessToken) { return Read<T>(resource, null, accessToken); }

        public static T Read<T>(string resource) { return Read<T>(resource, null, null); }

        private static T Read<T>(string resource, NetworkCredential credential, string accessToken)
        {
            var client = new RestClient(Server.BaseUrl);
            var request = new RestRequest(resource);

            // Add Authentication Header Data
            if (credential != null) client.Authenticator = new HttpBasicAuthenticator(credential.UserName, credential.Password);

            // Add Access Token as URL Parameter
            if (!string.IsNullOrEmpty(accessToken)) request.AddQueryParameter("access_token", accessToken);

            // Execute Response
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.StatusCode);
            if (response.StatusCode == HttpStatusCode.OK) {

                if (typeof(T) != typeof(string))
                {
                    return FromJson<T>(response.Content);
                }
                else return (T)Convert.ChangeType(response.Content, typeof(string)); // All that just to convert to string
            }

            return default(T);
        }

        #endregion

        #region "Send"

        public static bool Send(object obj, string resource, NetworkCredential credential) { return Send(obj, resource, credential, null); }

        public static bool Send(object obj, string resource, string accessToken) { return Send(obj, resource, null, accessToken); }

        public static bool Send(object obj, string resource) { return Send(obj, resource, null, null); }

        private static bool Send(object obj, string resource, NetworkCredential credential, string accessToken)
        {
            string json = ToJson(obj);
            if (!string.IsNullOrEmpty(json))
            {
                Console.WriteLine("JSON Size = " + json.Length);

                var client = new RestClient(Server.BaseUrl);
                var request = new RestRequest(resource);
                request.Method = Method.POST;

                // Add Authentication Header Data
                if (credential != null) client.Authenticator = new HttpBasicAuthenticator(credential.UserName, credential.Password);

                // Add Access Token as URL Parameter
                if (!string.IsNullOrEmpty(accessToken)) request.AddQueryParameter("access_token", accessToken);

                // Add POST data
                request.AddParameter("samples", json);

                var stpw = new System.Diagnostics.Stopwatch();
                stpw.Start();

                // Execute Response
                IRestResponse response = client.Execute(request);

                stpw.Stop();

                Console.WriteLine("Send() : " + stpw.ElapsedMilliseconds + "ms : " + response.StatusCode);

                return response.StatusCode == HttpStatusCode.OK;
            }

            return false;
        }

        #endregion

        #region "Json"

        public static T FromJson<T>(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    var jss = new JsonSerializerSettings();
                    jss.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    jss.DateParseHandling = DateParseHandling.DateTime;
                    jss.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    jss.NullValueHandling = NullValueHandling.Ignore;

                    return (T)JsonConvert.DeserializeObject(json, (typeof(T)), jss);
                }
                catch (JsonException jex) { Console.WriteLine(jex.Message); }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }

            return default(T);
        }

        public static string ToJson(object data)
        {
            try
            {
                var jss = new JsonSerializerSettings();
                jss.NullValueHandling = NullValueHandling.Ignore;

                return JsonConvert.SerializeObject(data, jss);
            }
            catch (JsonException jex) { Console.WriteLine(jex.Message); }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return null;
        }

        #endregion

    }
}

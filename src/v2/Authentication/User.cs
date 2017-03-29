// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;

namespace TrakHound.Api.v2.Authentication
{
    public class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("api_key")]
        public ApiKey ApiKey { get; set; }

        [JsonProperty("last_login")]
        public DateTime LastLogin { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }


        public static User Get(string username, string password)
        {
            var client = new RestClient(Server.BaseUrl);
            var request = new RestRequest("user", Method.GET);

            var authenticator = new HttpBasicAuthenticator(username, password);
            authenticator.Authenticate(client, request);

            var response = client.Execute(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = response.Content;
                if (!string.IsNullOrEmpty(json))
                {
                    var obj = Json.Convert.FromJson<User>(json);
                    if (obj != null) return obj;
                }
            }

            return null;
        }

    }
}

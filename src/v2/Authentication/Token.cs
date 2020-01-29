// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;

namespace TrakHound.Api.v2.Authentication
{
    public class Token
    {
        [JsonProperty("token")]
        public string Id { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("ttl")]
        public int TTL { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }


        public static Token Create(string username, string password)
        {
            var client = new RestClient(Server.BaseUrl);
            var request = new RestRequest("tokens", Method.POST);

            var authenticator = new HttpBasicAuthenticator(username, password);
            authenticator.Authenticate(client, request);

            var response = client.Execute(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = response.Content;
                if (!string.IsNullOrEmpty(json))
                {
                    var obj = Json.Convert.FromJson<Token>(json);
                    if (obj != null) return obj;
                }
            }

            return null;
        }

        public static Token Get(string tokenId)
        {
            var client = new RestClient(Server.BaseUrl);
            var request = new RestRequest("tokens/" + tokenId, Method.GET);

            var response = client.Execute(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = response.Content;
                if (!string.IsNullOrEmpty(json))
                {
                    var obj = Json.Convert.FromJson<Token>(json);
                    if (obj != null) return obj;
                }
            }

            return null;
        }

        public static bool Delete(string tokenId)
        {
            var client = new RestClient(Server.BaseUrl);
            var request = new RestRequest("tokens/" + tokenId, Method.DELETE);

            var response = client.Execute(request);
            return response != null && response.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}

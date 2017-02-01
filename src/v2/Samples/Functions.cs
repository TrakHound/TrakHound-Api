// Copyright (c) 2017 TrakHound Inc, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Net;

namespace TrakHound.Api.v2
{
    public partial class Samples
    {
        public static List<Container> Get(string resource, NetworkCredential credential)
        {
            var paths = new string[] { resource, "samples" };
            var path = string.Join("/", paths);
            return Requests.Read<List<Container>>(path, credential);
        }

        //public static bool Send(List<Container> containers, string resource, NetworkCredential credential)
        //{
        //    foreach (var container in containers)
        //    {
        //        Console.WriteLine("Sending Samples.." + container.DeviceId + " : "+ container.Samples.Count);
        //    }

        //    var paths = new string[] { resource, "samples" };
        //    var path = string.Join("/", paths);
        //    return Requests.Send(containers, path);
        //}
    }
}

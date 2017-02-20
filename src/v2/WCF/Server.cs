// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using NLog;
using System;
using System.ServiceModel;

namespace TrakHound.Api.v2.WCF
{
    public static class Server
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public static ServiceHost Create<T>()
        {
            return Create<T>(typeof(IMessage));
        }

        public static ServiceHost Create<T>(string pipeName)
        {
            return Create<T>(typeof(IMessage), pipeName);
        }

        public static ServiceHost Create<T>(Type interfaceType)
        {
            return Create<T>(interfaceType);
        }

        public static ServiceHost Create<T>(Type interfaceType, string pipeName)
        {
            var uri = new Uri("net.pipe://localhost/" + pipeName);

            log.Info("Opening Messaging Pipe @ " + uri);

            var security = new NetNamedPipeSecurity();
            security.Mode = NetNamedPipeSecurityMode.None;
            security.Transport = new NamedPipeTransportSecurity() { ProtectionLevel = System.Net.Security.ProtectionLevel.None };

            var binding = new NetNamedPipeBinding();
            binding.Security = security;

            var host = new ServiceHost(typeof(T));
            host.AddServiceEndpoint(interfaceType, binding, uri);
            host.Open();

            log.Info("WCF Host Opened @ " + uri);

            return host;
        }
    }
}

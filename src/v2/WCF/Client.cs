// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using NLog;
using System;
using System.ServiceModel;

namespace TrakHound.Api.v2.WCF
{
    public class MessageClient : IMessageCallback
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        private IMessage messageProxy;

        public MessageClient(string pipename)
        {
            string uri = "net.pipe://localhost/" + pipename;

            log.Info("Sending Message to " + uri);

            var security = new NetNamedPipeSecurity();
            security.Mode = NetNamedPipeSecurityMode.None;
            security.Transport = new NamedPipeTransportSecurity() { ProtectionLevel = System.Net.Security.ProtectionLevel.None };

            var binding = new NetNamedPipeBinding();
            binding.Security = security;

            //var pipeFactory = new ChannelFactory<IMessage>(binding, new EndpointAddress(uri));

            DuplexChannelFactory<IMessage> pipeFactory =
                    new DuplexChannelFactory<IMessage>(
                    new InstanceContext(this),
                    binding,
                    new EndpointAddress(uri));

            messageProxy = pipeFactory.CreateChannel();
        }

        public static void Send(string pipename, Message data)
        {
            var client = new MessageClient(pipename);
            client.SendMessage(data);
        }

        public void SendMessage(Message data)
        {
            try
            {
                if (messageProxy != null) messageProxy.SendData(data);
            }
            catch (Exception ex)
            {
                log.Error("Error during MessageClient Send Operation");
                log.Trace(ex);
            }
        }

        public delegate void MessageRecieved_Handler(Message data);
        public event MessageRecieved_Handler MessageRecieved;

        public void OnCallback(Message data)
        {
            MessageRecieved?.Invoke(data);
        }
    }
}

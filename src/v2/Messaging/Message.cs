// Copyright (c) 2017 TrakHound Inc, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using NLog;
using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace TrakHound.Api.v2.Messaging
{
    public class Message
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public string Id { get; set; }

        public string Text { get; set; }


        public Message() { }

        public Message(string id, string text)
        {
            Id = id;
            Text = text;
        }

        public static void Send(string pipeName, string id, string text)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((o) =>
            {
                var message = new Message(id, text);
                if (message.Send(pipeName)) log.Info("Message sent to " + pipeName + " successfully");
                else log.Info("Error sending message to " + pipeName);
            }));
        }

        public bool Send(string pipeName)
        {
            try
            {
                using (var client = new NamedPipeClientStream(".", pipeName, PipeDirection.Out))
                using (var writer = new StreamWriter(client))
                {
                    client.Connect(5000);

                    string json = Json.Convert.ToJson(this);
                    if (!string.IsNullOrEmpty(json))
                    {
                        writer.AutoFlush = true;
                        writer.WriteLine(json);
                    }

                    client.WaitForPipeDrain();
                }
            }
            catch (Exception ex)
            {
                log.Trace(ex);
            }

            return false;
        }
    }
}

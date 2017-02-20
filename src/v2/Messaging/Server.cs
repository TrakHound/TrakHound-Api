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
    public class Server
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        private ManualResetEvent stop;


        public delegate void MessageHandler(Message message);
        public event MessageHandler MessageReceived;

        private string _pipeName;
        public string PipeName
        {
            get { return _pipeName; }
        }


        public Server(string pipeName)
        {
            _pipeName = pipeName;
        }

        public void Start()
        {
            stop = new ManualResetEvent(false);

            while (!stop.WaitOne(0, true))
            {
                try
                {
                    log.Trace("Starting Pipe Server at " + PipeName);
                    // Create NamedPipe connection
                    using (var server = new NamedPipeServerStream(_pipeName, PipeDirection.In))
                    {
                        log.Trace("Waiting for Connection at " + PipeName);

                        // Wait for a new connection
                        server.WaitForConnection();

                        using (var reader = new StreamReader(server))
                        {
                            string json = reader.ReadLine();
                            if (!string.IsNullOrEmpty(json))
                            {
                                log.Trace("Pipe Data Received : " + json);

                                var message = Json.Convert.FromJson<Message>(json);
                                if (message != null) MessageReceived?.Invoke(message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Trace(ex);
                }
            }
        }

        public void Stop()
        {
            if (stop != null) stop.Set();
        }
    }
}

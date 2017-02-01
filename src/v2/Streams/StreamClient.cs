// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace TrakHound.Api.v2.Streams
{
    /// <summary>
    /// Client class for streaming data to a TrakHound Data Server
    /// </summary>
    public class StreamClient
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private object _lock = new object();
        private Thread thread;
        private ManualResetEvent stop;

        private TcpClient client;
        private Stream stream;
        private StreamWriter streamWriter;
        private StreamReader streamReader;
        private List<IStreamData> queue = new List<IStreamData>();
        private bool connected;

        private string _serverHostname;
        /// <summary>
        /// Data Server hostname to send sample data to
        /// </summary>
        public string ServerHostname { get { return _serverHostname; } }

        private bool _useSSL;
        public bool UseSSL { get { return _useSSL; } }

        private int _port;
        public int Port { get { return _port; } }


        public event EventHandler Connected;
        public event EventHandler Disconnected;

        public delegate void SendSuccessfulHandler(int successfulCount);
        public event SendSuccessfulHandler SendSuccessful;

        public delegate void SendFailedHandler(List<IStreamData> streamData);
        public event SendFailedHandler SendFailed;

        public int Timeout { get; set; }
        public int ReconnectionDelay { get; set; }

        public StreamClient(string serverHostname)
        {
            Init();
            _serverHostname = serverHostname;
        }

        public StreamClient(string serverHostname, int port, bool useSSL)
        {
            Init();
            _port = port;
            _serverHostname = serverHostname;
            _useSSL = useSSL;
        }

        private void Init()
        {
            ReconnectionDelay = 2000;
            Timeout = 5000;
            _port = 8472;
        }

        public void Start()
        {
            stop = new ManualResetEvent(false);

            thread = new Thread(new ThreadStart(Worker));
            thread.Start();
        }


        public void Close()
        {
            if (stop != null) stop.Set();

            if (stream != null) stream.Close();
            if (client != null) client.Close();
        }

        /// <summary>
        /// Write a single Sample to the Data Server
        /// </summary>
        public void Write(IStreamData data)
        {
            var l = new List<IStreamData>() { data };
            Write(l);
        }

        /// <summary>
        /// Write a List of Samples to the Data Server
        /// </summary>
        public void Write(List<IStreamData> data)
        {
            if (data != null && data.Count > 0)
            {
                lock (_lock)
                {
                    // Add StreamData to Queue
                    queue.AddRange(data);

                    // Send pulse to notifiy worker thread that new items are in Queue
                    Monitor.Pulse(_lock);
                }
            }
        }

        private void Worker()
        {
            lock (_lock)
            {
                int delay = 0;

                do
                {
                    var sendList = new List<IStreamData>();

                    lock (_lock)
                    {
                        // Wait till pulse is sent to signal that new items are in queue
                        Monitor.Wait(_lock);

                        sendList.AddRange(queue);
                        queue.Clear();
                    }

                    if (sendList.Count > 0)
                    {
                        delay = 0;

                        // Connect to TCP Client
                        if (ConnectToClient())
                        {
                            var successfullySent = new List<IStreamData>();
                            var failedToSend = new List<IStreamData>();

                            // Send each Item
                            foreach (var item in sendList)
                            {
                                var responseCode = SendData(item);
                                if (responseCode == 200) successfullySent.Add(item);
                                else failedToSend.Add(item);
                            }

                            // Raise Events
                            if (successfullySent.Count > 0) SendSuccessful?.Invoke(successfullySent.Count);
                            if (failedToSend.Count > 0) SendFailed?.Invoke(failedToSend);
                        }
                        else
                        {
                            delay = ReconnectionDelay;
                            SendFailed?.Invoke(sendList);
                        }
                    }
                } while (!stop.WaitOne(delay, true));
            }
        }

        private bool ConnectToClient()
        {
            try
            {
                // Connect to TcpClient
                if (client == null || !client.Connected)
                {
                    // Create a new TcpClient
                    client = new TcpClient(_serverHostname, _port);
                    client.ReceiveTimeout = Timeout;
                    client.SendTimeout = Timeout;

                    // Get Stream
                    if (_useSSL)
                    {
                        // Create new SSL Stream from client's NetworkStream
                        var sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                        sslStream.AuthenticateAsClient(_serverHostname);
                        PrintCertificateInfo(sslStream);

                        stream = Stream.Synchronized(sslStream);
                    }
                    else
                    {
                        stream = Stream.Synchronized(client.GetStream());
                    }

                    streamWriter = new StreamWriter(stream);
                    streamReader = new StreamReader(stream);

                    logger.Info("Connection Established with " + ServerHostname + ":" + _port);
                    Connected?.Invoke(this, new EventArgs());
                    connected = true;
                }

                return true;
            }
            catch (Exception ex)
            {
                if (client != null) client.Close();
                logger.Warn("Error Connecting to " + ServerHostname + ":" + _port + ". Retrying in " + ReconnectionDelay + "ms..");
                logger.Trace(ex);
            }

            if (connected) Disconnected?.Invoke(this, new EventArgs());

            return false;
        }

        private int SendData(IStreamData data)
        {
            if (stream != null && data != null)
            {
                try
                {
                    var json = Requests.ToJson(data);
                    if (!string.IsNullOrEmpty(json))
                    {
                        // Write JSON to stream
                        streamWriter.WriteLine(json);
                        streamWriter.Flush();

                        // Read Response Code
                        var response = streamReader.ReadLine();
                        if (!string.IsNullOrEmpty(response))
                        {
                            int responseCode;
                            if (int.TryParse(response, out responseCode))
                            {
                                logger.Trace(response);
                                return responseCode;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Warn("Error While Sending Data. Send Failed.");
                    logger.Trace(ex);
                }
            }

            return -1;
        }

        private void PrintCertificateInfo(SslStream stream)
        {
            // Local
            var local = stream.LocalCertificate;
            if (local != null) PrintCertificateInfo(local, "Local");

            // Remote
            var remote = stream.RemoteCertificate;
            if (remote != null) PrintCertificateInfo(remote, "Remote");
        }

        private void PrintCertificateInfo(X509Certificate cert, string title = null)
        {
            logger.Trace("SSL Certificate Information (" + title + ")");
            logger.Trace("---------------------------");
            logger.Trace("Subject : " + cert.Subject);
            logger.Trace("Serial Number : " + cert.GetSerialNumber());
            logger.Trace("Format : " + cert.GetFormat());
            logger.Trace("Effective Date : " + cert.GetEffectiveDateString());
            logger.Trace("Expiration Date : " + cert.GetExpirationDateString());
            logger.Trace("---------------------------");
        }

        private static bool ValidateServerCertificate(
              object sender,
              X509Certificate certificate,
              X509Chain chain,
              SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            logger.Error("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }
    }

}

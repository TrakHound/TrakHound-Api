// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Timers;

namespace TrakHound.Api.v2
{
    public partial class Samples
    {
        /// <summary>
        /// Client class for streaming sample data to a TrakHound Data Server
        /// </summary>
        public class StreamingClient
        {
            private static Logger log = LogManager.GetCurrentClassLogger();

            private object _lock = new object();
            private bool waiting = false;

            private string _serverHostname;
            /// <summary>
            /// Data Server hostname to send sample data to
            /// </summary>
            public string ServerHostname { get { return _serverHostname; } }

            private bool _useSSL;
            public bool UseSSL { get { return _useSSL; } }

            private int _port;
            public int Port { get { return _port; } }

            private TcpClient _client;
            private Stream _stream;

            public int Timeout { get; set; }
            public int ReconnectionDelay { get; set; }

            public StreamingClient(string serverHostname)
            {
                Init();
                _serverHostname = serverHostname;
            }

            public StreamingClient(string serverHostname, int port, bool useSSL)
            {
                Init();
                _port = port;
                _serverHostname = serverHostname;
                _useSSL = useSSL;
            }

            private void Init()
            {
                ReconnectionDelay = 5000;
                Timeout = 5000;
                _port = 8472;
            }


            public void Close()
            {
                if (_stream != null) _stream.Close();
                if (_client != null) _client.Close();
            }

            /// <summary>
            /// Write a single Sample to the Data Server
            /// </summary>
            public bool Write(Sample sample)
            {
                var l = new List<Sample>() { sample };
                return Write(l);
            }

            /// <summary>
            /// Write a List of Samples to the Data Server
            /// </summary>
            public bool Write(List<Sample> samples)
            {
                lock (_lock)
                {
                    // Make sure not waiting to retry connection
                    if (waiting)
                    {
                        log.Trace("Waiting on Reconnection..");
                        return false;
                    }

                    try
                    {
                        // Connect to TcpClient
                        if (_client == null || !_client.Connected)
                        {
                            // Create a new TcpClient
                            _client = new TcpClient(_serverHostname, _port);
                            _client.ReceiveTimeout = Timeout;
                            _client.SendTimeout = Timeout;

                            // Get Stream
                            if (_useSSL)
                            {
                                // Create new SSL Stream from client's NetworkStream
                                var sslStream = new SslStream(_client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                                sslStream.AuthenticateAsClient(_serverHostname);
                                PrintCertificateInfo(sslStream);

                                _stream = Stream.Synchronized(sslStream);
                            }
                            else
                            {
                                _stream = Stream.Synchronized(_client.GetStream());
                            }

                            log.Info("Connection Established with " + ServerHostname + ":" + _port);
                        }

                        if (_client != null && _stream != null) // Not sure if needed since an exception should get thrown before reaching this
                        {
                            string json = Requests.ToJson(samples);
                            if (!string.IsNullOrEmpty(json))
                            {
                                byte[] b = Encoding.ASCII.GetBytes(json);
                                _stream.Write(b, 0, b.Length);

                                // Get Success Byte. This is mainly used to tell that JSON was in correct format
                                b = new byte[1];
                                if (_stream.Read(b, 0, b.Length) > 0)
                                {
                                    return b[0] == 101;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Warn("Error Connecting to " + ServerHostname + ":" + _port + ". Retrying in " + ReconnectionDelay + "ms..");
                        log.Trace(ex);
                        Wait(ReconnectionDelay);
                    }

                    return false;
                }
            }

            private void Wait(int ms)
            {
                lock (_lock) waiting = true;

                var timer = new Timer();
                timer.Interval = Math.Max(1000, ms);
                timer.Elapsed += Timer_Elapsed;
                timer.Enabled = true;
            }

            private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
            {
                var timer = (Timer)sender;
                timer.Enabled = false;

                lock (_lock) waiting = false;
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
                log.Trace("SSL Certificate Information (" + title + ")");
                log.Trace("---------------------------");
                log.Trace("Subject : " + cert.Subject);
                log.Trace("Serial Number : " + cert.GetSerialNumber());
                log.Trace("Format : " + cert.GetFormat());
                log.Trace("Effective Date : " + cert.GetEffectiveDateString());
                log.Trace("Expiration Date : " + cert.GetExpirationDateString());
                log.Trace("---------------------------");
            }

            private static bool ValidateServerCertificate(
                  object sender,
                  X509Certificate certificate,
                  X509Chain chain,
                  SslPolicyErrors sslPolicyErrors)
            {
                if (sslPolicyErrors == SslPolicyErrors.None)
                    return true;

                log.Error("Certificate error: {0}", sslPolicyErrors);

                // Do not allow this client to communicate with unauthenticated servers.
                return false;
            }

        }
    }
}

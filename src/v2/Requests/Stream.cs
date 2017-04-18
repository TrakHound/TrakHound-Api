// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.

using NLog;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;


namespace TrakHound.Api.v2.Requests
{
    public class Stream
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        private ManualResetEvent stop;
        private HttpWebRequest request;
        private System.IO.Stream requestStream;
        private StreamReader requestReader;


        public string Url { get; set; }
        public string StartCharacter { get; set; }
        public string EndCharacter { get; set; }

        public delegate void GroupHandler(string text);
        public event GroupHandler GroupReceived;


        public Stream(string url, string startCharacter, string endCharacter)
        {
            Url = url;
            StartCharacter = startCharacter;
            EndCharacter = endCharacter;
        }

        public void Start()
        {
            Stop();

            if (!string.IsNullOrEmpty(Url))
            {
                stop = new ManualResetEvent(false);

                ThreadPool.QueueUserWorkItem(new WaitCallback(Worker));
            }
        }

        public void Stop()
        {
            if (stop != null) stop.Set();
            if (request != null) request.Abort();
        }

        private void Worker(object o)
        {
            while (!stop.WaitOne(2000, true))
            {
                try
                {
                    var time = DateTime.UtcNow.ToUnixTime();

                    log.Debug("Request Stream : Connecting to : " + Url + " @ " + time);


                    request = (HttpWebRequest)WebRequest.Create(Url);
                    using (var response = (HttpWebResponse)request.GetResponse())
                    using (var requestStream = response.GetResponseStream())
                    using (var requestReader = new StreamReader(requestStream, Encoding.GetEncoding("utf-8")))
                    {
                        log.Debug("Request Stream : Connected to : " + Url + " @ " + time);

                        // Set Read Buffer
                        var buffer = new char[1024]; // 1 KB
                        int i = requestReader.Read(buffer, 0, buffer.Length);

                        string group = "";

                        while (i > 0 && !stop.WaitOne(0, true))
                        {
                            // Get string from buffer
                            var s = new string(buffer, 0, i);
                            var c = -1;

                            // Find Beginning of Group (exclude HTTP header)
                            int b = s.IndexOf(StartCharacter);
                            if (b >= 0) s = s.Substring(b);

                            // Add buffer to Group txt
                            group += s;

                            do
                            {
                                c = -1;

                                // Find Beginning of Group (exclude HTTP header)
                                b = s.IndexOf(StartCharacter);
                                if (b >= 0) s = s.Substring(b);

                                // Find Terminator Index
                                if (!string.IsNullOrEmpty(group))
                                {
                                    string tag = EndCharacter;
                                    c = group.IndexOf(tag);
                                    if (c >= 0) c = c + tag.Length;
                                }

                                // Test if End of Group
                                if (c >= 0)
                                {
                                    b = 0;
                                    if (c > b)
                                    {
                                        // Raise Group Received Event and pass text
                                        var txt = group.Substring(b, c - b).Trim();
                                        GroupReceived?.Invoke(txt);
                                    }

                                    // Remove from Group
                                    group = group.Remove(b, c - b);
                                }
                            } while (c > 0 && !stop.WaitOne(0, true));

                            // Clear Buffer
                            Array.Clear(buffer, 0, buffer.Length);

                            // Trim WhiteSpace (Leading/Trailing)
                            group = group.Trim();

                            // Read Next Chunk
                            i = requestReader.Read(buffer, 0, buffer.Length);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Debug(ex);
                }
            }
        }

    }
}

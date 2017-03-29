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
        private Thread thread;

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
            if (!string.IsNullOrEmpty(Url))
            {
                stop = new ManualResetEvent(false);

                thread = new Thread(new ThreadStart(Worker));
                thread.Start();
            }
        }

        public void Stop()
        {
            if (stop != null) stop.Set();
            if (request != null) request.Abort();
            if (requestReader != null) requestReader.Close();
            if (requestStream != null) requestStream.Dispose();
        }

        private void Worker()
        {
            try
            {
                if (!stop.WaitOne(0, true))
                {
                    request = (HttpWebRequest)WebRequest.Create(Url);
                    using (var response = (HttpWebResponse)request.GetResponse())
                    using (requestStream = response.GetResponseStream())
                    using (requestReader = new StreamReader(requestStream, Encoding.GetEncoding("utf-8")))
                    {
                        // Set Read Buffer
                        var buffer = new char[1048576]; // 1 MB
                        int i = requestReader.Read(buffer, 0, buffer.Length);

                        string group = "";

                        while (i > 0 && !stop.WaitOne(0, true))
                        {
                            // Get string from buffer
                            var s = new string(buffer, 0, i);

                            // Find Beginning of Group (exclude HTTP header)
                            int b = s.IndexOf(StartCharacter);
                            if (b >= 0) s = s.Substring(b);

                            // Add buffer to Group txt
                            group += s;

                            // Find Terminator Index
                            int c = -1;
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
                                    GroupReceived?.Invoke(group.Substring(b, c - b));
                                }

                                // Reset Group Text
                                group = "";
                            }

                            // Clear Buffer
                            Array.Clear(buffer, 0, buffer.Length);

                            // Trim WhiteSpace (Leading/Trailing)
                            group.Trim();

                            // Read Next Chunk
                            i = requestReader.Read(buffer, 0, buffer.Length);
                        }
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

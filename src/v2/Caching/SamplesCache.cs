// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Linq;
using TrakHound.Api.v2.Data;

namespace TrakHound.Api.v2.Caching
{
    public class SamplesCache
    {
        private static System.Timers.Timer cleanUpTimer = CreateCleanUpTimer();
        private static object _lock = new object();
        private static List<CachedSample> cache = new List<CachedSample>();

        public static List<Sample> Read(string deviceId, string[] ids, DateTime from, DateTime to)
        {
            if (!ids.IsNullOrEmpty())
            {
                var samples = new List<Sample>();

                foreach (var id in ids)
                {
                    var cachedSamples = new List<Sample>();
                    var firstCached = DateTime.MinValue;
                    var lastCached = DateTime.MinValue;

                    List<CachedSample> objs = null;

                    lock (_lock)
                    {
                        // Get initial sample at From timestamp from Cache
                        objs = cache.FindAll(o => o.Sample.DeviceId == deviceId && o.Sample.Id == id && o.Sample.Timestamp <= from);
                        if (!objs.IsNullOrEmpty())
                        {
                            var initial = objs.OrderBy(o => o.Sample.Timestamp).Last();
                            if (initial != null) cachedSamples.Add(initial.Sample);
                        }

                        // Find any other samples within timestamps (to & from)
                        objs = cache.FindAll(o => o.Sample.DeviceId == deviceId && o.Sample.Timestamp > from && o.Sample.Timestamp <= to);
                    }
                    
                    foreach (var obj in objs) obj.LastUsed = DateTime.UtcNow;
                    cachedSamples.AddRange(objs.Select(o => o.Sample).ToList());

                    if (!objs.IsNullOrEmpty())
                    {
                        var orderedSamples = objs.OrderBy(o => o.Sample.Timestamp);

                        // Find first Sample in cache
                        firstCached = orderedSamples.First().Sample.Timestamp;

                        // Find last Sample in cache
                        lastCached = orderedSamples.Last().Sample.Timestamp;
                    }

                    var readSamples = new List<Sample>();

                    // Read all new samples (no cached samples found)
                    if (firstCached == DateTime.MinValue && lastCached == DateTime.MinValue)
                    {
                        var s = Database.ReadSamples(ids.ToArray(), deviceId, from, to, DateTime.MinValue, long.MaxValue);
                        if (!s.IsNullOrEmpty()) readSamples.AddRange(s);
                    }
                    else
                    {
                        // Get Samples before cache
                        if (firstCached > from)
                        {
                            var s = Database.ReadSamples(ids.ToArray(), deviceId, from, firstCached, DateTime.MinValue, long.MaxValue);
                            if (!s.IsNullOrEmpty()) readSamples.AddRange(s);
                        }

                        // Get Samples after cache 
                        if (lastCached < to)
                        {
                            var s = Database.ReadSamples(ids.ToArray(), deviceId, lastCached, to, DateTime.MinValue, long.MaxValue);
                            if (!s.IsNullOrEmpty()) readSamples.AddRange(s);
                        }
                    }

                    samples.AddRange(readSamples);

                    // Write any newly read samples to cache
                    if (!readSamples.IsNullOrEmpty())
                    {
                        var newSamples = readSamples.FindAll(o => o.Timestamp > lastCached);
                        if (!newSamples.IsNullOrEmpty())
                        {
                            samples.AddRange(newSamples);

                            foreach (var sample in newSamples)
                            {
                                lock (_lock) cache.Add(new CachedSample(sample));
                            }
                        }
                    }

                    if (!cachedSamples.IsNullOrEmpty()) samples.AddRange(cachedSamples);
                }

                if (!samples.IsNullOrEmpty())
                {
                    samples = samples.OrderBy(o => o.Timestamp).ToList();
                }

                return samples;
            }

            return null;
        }

        private static System.Timers.Timer CreateCleanUpTimer()
        {
            var timer = new System.Timers.Timer();
            timer.Interval = 10000;
            timer.Elapsed += CleanUpTimer_Elapsed;
            timer.Start();
            return timer;
        }

        private static void CleanUpTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (_lock)
            {
                // Clear cache of any items that haven't been used in over 15 minutes
                cache.RemoveAll(o => (DateTime.UtcNow - o.LastUsed) > TimeSpan.FromMinutes(15));
            }
        }
    }
}

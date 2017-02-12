// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Collections.Generic;

namespace TrakHound.Api.v2.Data
{
    public class SampleInfo : Sample
    {
        public DataItemInfo DataItem { get; set; }

        public SampleInfo(Sample sample, DataItemInfo dataItem)
        {
            DeviceId = sample.DeviceId;
            Id = sample.Id;
            Timestamp = sample.Timestamp;
            Sequence = sample.Sequence;
            AgentInstanceId = sample.AgentInstanceId;
            CDATA = sample.CDATA;
            Condition = sample.Condition;
            DataItem = dataItem;
        }

        public static SampleInfo Create(List<DataItemInfo> dataItemInfos, Sample sample)
        {
            if (sample != null && !dataItemInfos.IsNullOrEmpty())
            {
                var dataItem = dataItemInfos.Find(o => o.Id == sample.Id);
                if (dataItem != null) return new SampleInfo(sample, dataItem);
            }

            return null;
        }

        public static List<SampleInfo> Create(List<DataItemInfo> dataItemInfos, List<Sample> samples)
        {
            if (samples.IsNullOrEmpty()) return null;

            var sampleInfoList = new List<SampleInfo>();
            foreach (Sample sample in samples)
            {
                var sampleInfo = Create(dataItemInfos, sample);
                if (sampleInfo != null) sampleInfoList.Add(sampleInfo);
            }

            return sampleInfoList;
        }
    }
}

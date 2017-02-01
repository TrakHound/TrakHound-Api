// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;

namespace TrakHound.Api.v2.Data
{
    public class SampleInfo : Sample
    {
        public DataItemInfo DataItem { get; set; }

        public SampleInfo(Sample sample, DataItemInfo dataItem)
        {
            this.DeviceId = sample.DeviceId;
            this.Id = sample.Id;
            this.Timestamp = sample.Timestamp;
            this.Sequence = sample.Sequence;
            this.AgentInstanceId = sample.AgentInstanceId;
            this.CDATA = sample.CDATA;
            this.Condition = sample.Condition;
            this.DataItem = dataItem;
        }

        public static SampleInfo Create(List<DataItemInfo> dataItemInfos, Sample sample)
        {
            if (sample != null && !dataItemInfos.IsNullOrEmpty<DataItemInfo>())
            {
                DataItemInfo dataItem = dataItemInfos.Find((Predicate<DataItemInfo>)(o => o.Id == sample.Id));
                if (dataItem != null)
                    return new SampleInfo(sample, dataItem);
            }
            return (SampleInfo)null;
        }

        public static List<SampleInfo> Create(List<DataItemInfo> dataItemInfos, List<Sample> samples)
        {
            if (samples.IsNullOrEmpty<Sample>())
                return (List<SampleInfo>)null;
            List<SampleInfo> sampleInfoList = new List<SampleInfo>();
            foreach (Sample sample in samples)
            {
                SampleInfo sampleInfo = SampleInfo.Create(dataItemInfos, sample);
                if (sampleInfo != null)
                    sampleInfoList.Add(sampleInfo);
            }
            return sampleInfoList;
        }
    }
}

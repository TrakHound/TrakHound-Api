// Copyright (c) 2019 TrakHound Inc, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace TrakHound.Api.v2.Events
{
    public class ResponseValue
    {
        public string DataItemId { get; set; }

        public string Value { get; set; }

        public DateTime Timestamp { get; set; }


        public ResponseValue() { }

        public ResponseValue(string dataItemId)
        {
            DataItemId = dataItemId;
        }
    }
}

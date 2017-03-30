// Copyright (c) 2017 TrakHound Inc, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace TrakHound.Api.v2.Streams
{
    public enum StreamDataType
    {
        UNKNOWN,

        CONNECTION_DEFINITION,
        AGENT_DEFINITION,
        DEVICE_DEFINITION,
        COMPONENT_DEFINITION,
        DATA_ITEM_DEFINITION,

        ARCHIVED_SAMPLE,
        CURRENT_SAMPLE,

        STATUS,

        ASSET_DEFINITION
    }
}

// Copyright (c) 2017 TrakHound Inc, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace TrakHound.Api.v2.Events
{
    public enum TriggerModifier
    {
        None = 0,
        NOT = 1,
        GREATER_THAN = 2,
        LESS_THAN = 3,
        CONTAINS = 4,
        CONTAINS_MATCH_CASE = 5,
        CONTAINS_WHOLE_WORD = 6,
        CONTAINS_WHOLE_WORD_MATCH_CASE = 7
    };
}

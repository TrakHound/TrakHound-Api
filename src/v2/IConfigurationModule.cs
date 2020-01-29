// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace TrakHound.Api.v2
{
    public interface IConfigurationModule
    {
        string Name { get; }

        bool Configure();

        bool Configure(string deviceId);
    }
}

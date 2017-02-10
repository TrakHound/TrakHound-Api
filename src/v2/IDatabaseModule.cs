// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using TrakHound.Api.v2.Data;
using TrakHound.Api.v2.Streams.Data;

namespace TrakHound.Api.v2
{
    /// <summary>
    /// Interface for Database Module
    /// </summary>
    public interface IDatabaseModule
    {
        /// <summary>
        /// Gets the name of the Database. This corresponds to the node name in the 'server.config' file
        /// </summary>
        string Name { get; }

        bool Initialize(string databaseConfigurationPath);

        #region "Read"

        /// <summary>
        /// Read the ConnectionDefinition from the database
        /// </summary>
        ConnectionDefinition ReadConnection(string deviceId);

        /// <summary>
        /// Read the most current AgentDefintion from the database
        /// </summary>
        AgentDefinition ReadAgent(string deviceId);

        /// <summary>
        /// Read the ComponentDefinitions for the specified Agent Instance Id from the database
        /// </summary>
        List<ComponentDefinition> ReadComponents(string deviceId, long agentInstanceId);

        /// <summary>
        /// Read the DataItemDefinitions for the specified Agent Instance Id from the database
        /// </summary>
        List<DataItemDefinition> ReadDataItems(string deviceId, long agentInstanceId);

        /// <summary>
        /// Read the DeviceDefintion for the specified Agent Instance Id from the database
        /// </summary>
        DeviceDefinition ReadDevice(string deviceId, long agentInstanceId);

        /// <summary>
        /// Read Samples from the database
        /// </summary>
        List<Sample> ReadSamples(string[] dataItemIds, string deviceId, DateTime from, DateTime to, DateTime at, long count);

        #endregion

        #region "Write"

        /// <summary>
        /// Write ConnectionDefintions to the database
        /// </summary>
        bool Write(List<ConnectionDefinitionData> definitions);

        /// <summary>
        /// Write AgentDefintions to the database
        /// </summary>
        bool Write(List<AgentDefinitionData> definitions);

        /// <summary>
        /// Write ComponentDefintions to the database
        /// </summary>
        bool Write(List<ComponentDefinitionData> definitions);

        /// <summary>
        /// Write DataItemDefintions to the database
        /// </summary>
        bool Write(List<DataItemDefinitionData> definitions);

        /// <summary>
        /// Write DeviceDefintions to the database
        /// </summary>
        bool Write(List<DeviceDefinitionData> definitions);

        /// <summary>
        /// Write Samples to the database
        /// </summary>
        bool Write(List<SampleData> samples);

        #endregion
    }
}

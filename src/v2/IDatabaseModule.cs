// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using TrakHound.Api.v2.Configurations;
using TrakHound.Api.v2.Data;

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

        bool Initialize(DatabaseConfiguration config);

        void Close();

        /// <summary>
        /// Execute a custom query and return whether the query was successful
        /// </summary>
        bool ExecuteQuery(string query, int timeout = 0);

        /// <summary>
        /// Execute a custom query and return an object of the specified type
        /// </summary>
        T Read<T>(string query, int timeout = 0);

        /// <summary>
        /// Execute a custom query and return a List of objects of the specified type
        /// </summary>
        List<T> ReadList<T>(string query, int timeout = 0);

        /// <summary>
        /// Read all of the Connections available from the DataServer
        /// </summary>
        List<ConnectionDefinition> ReadConnections();

        /// <summary>
        /// Read the ConnectionDefinition from the database
        /// </summary>
        ConnectionDefinition ReadConnection(string deviceId);

        /// <summary>
        /// Read the most current AgentDefintion from the database
        /// </summary>
        AgentDefinition ReadAgent(string deviceId);

        /// <summary>
        /// Read AssetDefintions from the database
        /// </summary>
        List<AssetDefinition> ReadAssets(string deviceId, string assetId, DateTime from, DateTime to, DateTime at, long count);

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
        List<Sample> ReadSamples(string[] dataItemIds, string deviceId, DateTime from, DateTime to, DateTime at, long count, bool includeCurrent);

        /// <summary>
        /// Read the Status from the database
        /// </summary>
        Status ReadStatus(string deviceId);


        bool UpdateSetting(string name, string value);

        string ReadSetting(string name);
    }
}

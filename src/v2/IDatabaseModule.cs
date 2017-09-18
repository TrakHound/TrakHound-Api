// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
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

        bool Initialize(string databaseConfigurationPath);

        void Close();

        #region "Read"

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
        List<Sample> ReadSamples(string[] dataItemIds, string deviceId, DateTime from, DateTime to, DateTime at, long count);

        /// <summary>
        /// Read RejectedParts from the database
        /// </summary>
        List<RejectedPart> ReadRejectedParts(string deviceId, string[] partIds, DateTime from, DateTime to, DateTime at);

        /// <summary>
        /// Read Verified from the database
        /// </summary>
        List<VerifiedPart> ReadVerifiedParts(string deviceId, string[] partIds, DateTime from, DateTime to, DateTime at);

        /// <summary>
        /// Read the Status from the database
        /// </summary>
        Status ReadStatus(string deviceId);

        #endregion

        #region "Write"

        /// <summary>
        /// Write ConnectionDefintions to the database
        /// </summary>
        bool Write(List<ConnectionDefinition> definitions);

        /// <summary>
        /// Write AgentDefintions to the database
        /// </summary>
        bool Write(List<AgentDefinition> definitions);

        /// <summary>
        /// Write AssetDefintions to the database
        /// </summary>
        bool Write(List<AssetDefinition> definitions);

        /// <summary>
        /// Write ComponentDefintions to the database
        /// </summary>
        bool Write(List<ComponentDefinition> definitions);

        /// <summary>
        /// Write DataItemDefintions to the database
        /// </summary>
        bool Write(List<DataItemDefinition> definitions);

        /// <summary>
        /// Write DeviceDefintions to the database
        /// </summary>
        bool Write(List<DeviceDefinition> definitions);

        /// <summary>
        /// Write Samples to the database
        /// </summary>
        bool WriteArchivedSamples(List<Sample> samples);

        /// <summary>
        /// Write Samples to the database
        /// </summary>
        bool WriteCurrentSamples(List<Sample> samples);

        /// <summary>
        /// Write RejectedParts to the database
        /// </summary>
        bool Write(List<RejectedPart> parts);

        /// <summary>
        /// Write VerifiedParts to the database
        /// </summary>
        bool Write(List<VerifiedPart> parts);

        /// <summary>
        /// Write Statuses to the database
        /// </summary>
        bool Write(List<Status> statuses);

        #endregion

        #region "Delete"

        /// <summary>
        /// Delete Rejected Part from the database
        /// </summary>
        bool DeleteRejectedPart(string deviceId, string partId);

        /// <summary>
        /// Delete Verified Part from the database
        /// </summary>
        bool DeleteVerifiedPart(string deviceId, string partId);

        #endregion
    }
}

// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using TrakHound.Api.v2.Data;
using TrakHound.Api.v2.Streams.Data;

namespace TrakHound.Api.v2
{
    public static class Database
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The currently loaded IDatabaseModule
        /// </summary>
        public static IDatabaseModule Module;


        public static bool Initialize(string configurationPath, string modulesDirectory = null)
        {
            var assemblyDir = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            var path = configurationPath;
            if (!Path.IsPathRooted(path)) path = Path.Combine(assemblyDir, configurationPath);

            if (!string.IsNullOrEmpty(path))
            {
                log.Info("Reading Database Configuration File From '" + path + "'");

                var modulesDir = assemblyDir;
                if (!string.IsNullOrEmpty(modulesDirectory))
                {
                    modulesDir = modulesDirectory;
                    if (!Path.IsPathRooted(modulesDir)) modulesDir = Path.Combine(assemblyDir, modulesDir);
                }

                log.Info("Reading Database Modules From '" + modulesDir + "'");

                var modules = FindModules(modulesDir);
                if (modules != null)
                {
                    foreach (var module in modules)
                    {
                        try
                        {
                            if (module.Initialize(path))
                            {
                                log.Info(module.Name + " Database Module Initialize Successfully");
                                Module = module;
                                return true;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Trace(ex);
                        }
                    }
                }
            }

            return false;
        }

        public static void Close()
        {
            if (Module != null)
            {
                try
                {
                    Module.Close();
                }
                catch (Exception ex)
                {
                    log.Trace(ex);
                }
            }           
        }

        #region "Modules"

        private class ModuleContainer
        {
            [ImportMany(typeof(IDatabaseModule))]
            public IEnumerable<Lazy<IDatabaseModule>> Modules { get; set; }
        }

        private static List<IDatabaseModule> FindModules(string dir)
        {
            if (dir != null)
            {
                if (Directory.Exists(dir))
                {
                    var catalog = new DirectoryCatalog(dir);
                    var container = new CompositionContainer(catalog);
                    return FindModules(container);
                }
            }

            return null;
        }

        private static List<IDatabaseModule> FindModules(Assembly assembly)
        {
            if (assembly != null)
            {
                var catalog = new AssemblyCatalog(assembly);
                var container = new CompositionContainer(catalog);
                return FindModules(container);
            }

            return null;
        }

        private static List<IDatabaseModule> FindModules(CompositionContainer container)
        {
            try
            {
                var moduleContainer = new ModuleContainer();
                container.SatisfyImportsOnce(moduleContainer);

                if (moduleContainer.Modules != null)
                {
                    var modules = new List<IDatabaseModule>();

                    foreach (var lModule in moduleContainer.Modules)
                    {
                        try
                        {
                            var module = lModule.Value;
                            modules.Add(module);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex, "Module Initialization Error");
                        }
                    }

                    return modules;
                }
            }
            catch (ReflectionTypeLoadException ex) { log.Error(ex); }
            catch (UnauthorizedAccessException ex) { log.Error(ex); }
            catch (Exception ex) { log.Error(ex); }

            return null;
        }

        #endregion

        #region "Read"

        public static List<T> ExecuteQuery<T>(string query)
        {
            if (Module != null) return Module.ExecuteQuery<T>(query);

            return null;
        }

        /// <summary>
        /// Read all of the Connections available from the DataServer
        /// </summary>
        public static List<ConnectionDefinition> ReadConnections()
        {
            if (Module != null) return Module.ReadConnections();

            return null;
        }

        /// <summary>
        /// Read the ConnectionDefintion from the database
        /// </summary>
        public static ConnectionDefinition ReadConnection(string deviceId)
        {
            if (Module != null) return Module.ReadConnection(deviceId);

            return null;
        }

        /// <summary>
        /// Read the most current AgentDefintion from the database
        /// </summary>
        public static AgentDefinition ReadAgent(string deviceId)
        {
            if (Module != null) return Module.ReadAgent(deviceId);

            return null;
        }

        /// <summary>
        /// Read AssetDefintions from the database
        /// </summary>
        public static List<AssetDefinition> ReadAssets(string deviceId, string assetId, DateTime from, DateTime to, DateTime at, long count)
        {
            if (Module != null) return Module.ReadAssets(deviceId, assetId, from, to, at, count);

            return null;
        }

        /// <summary>
        /// Read the ComponentDefinitions for the specified Agent Instance Id from the database
        /// </summary>
        public static List<ComponentDefinition> ReadComponents(string deviceId, long agentInstanceId)
        {
            if (Module != null) return Module.ReadComponents(deviceId, agentInstanceId);

            return null;
        }

        /// <summary>
        /// Read the DataItemDefinitions for the specified Agent Instance Id from the database
        /// </summary>
        public static List<DataItemDefinition> ReadDataItems(string deviceId, long agentInstanceId)
        {
            if (Module != null) return Module.ReadDataItems(deviceId, agentInstanceId);

            return null;
        }

        /// <summary>
        /// Read the DeviceDefintion for the specified Agent Instance Id from the database
        /// </summary>
        public static DeviceDefinition ReadDevice(string deviceId, long agentInstanceId)
        {
            if (Module != null) return Module.ReadDevice(deviceId, agentInstanceId);

            return null;
        }

        /// <summary>
        /// Read Samples from the database
        /// </summary>
        public static List<Sample> ReadSamples(string[] dataItemIds, string deviceId, DateTime from, DateTime to, DateTime at, long count, bool includeCurrent)
        {
            if (Module != null) return Module.ReadSamples(dataItemIds, deviceId, from, to, at, count, includeCurrent);

            return null;
        }

        /// <summary>
        /// Read RejectedParts from the database
        /// </summary>
        public static List<RejectedPart> ReadRejectedParts(string deviceId, string[] partIds, DateTime from, DateTime to, DateTime at)
        {
            if (Module != null) return Module.ReadRejectedParts(deviceId, partIds, from, to, at);

            return null;
        }

        /// <summary>
        /// Read VerifiedParts from the database
        /// </summary>
        public static List<VerifiedPart> ReadVerifiedParts(string deviceId, string[] partIds, DateTime from, DateTime to, DateTime at)
        {
            if (Module != null) return Module.ReadVerifiedParts(deviceId, partIds, from, to, at);

            return null;
        }

        /// <summary>
        /// Read the Status from the database
        /// </summary>
        public static Status ReadStatus(string deviceId)
        {
            if (Module != null) return Module.ReadStatus(deviceId);

            return null;
        }

        #endregion

        #region "Write"

        /// <summary>
        /// Write ConnectionDefinitions to the database
        /// </summary>
        public static bool Write(List<ConnectionDefinition> definitions)
        {
            if (Module != null) return Module.Write(definitions);

            return false;
        }

        /// <summary>
        /// Write AgentDefintions to the database
        /// </summary>
        public static bool Write(List<AgentDefinition> definitions)
        {
            if (Module != null) return Module.Write(definitions);

            return false;
        }

        /// <summary>
        /// Write AssetDefintions to the database
        /// </summary>
        public static bool Write(List<AssetDefinition> definitions)
        {
            if (Module != null) return Module.Write(definitions);

            return false;
        }

        /// <summary>
        /// Write ComponentDefintions to the database
        /// </summary>
        public static bool Write(List<ComponentDefinition> definitions)
        {
            if (Module != null) return Module.Write(definitions);

            return false;
        }

        /// <summary>
        /// Write DataItemDefintions to the database
        /// </summary>
        public static bool Write(List<DataItemDefinition> definitions)
        {
            if (Module != null) return Module.Write(definitions);

            return false;
        }

        /// <summary>
        /// Write DeviceDefintions to the database
        /// </summary>
        public static bool Write(List<DeviceDefinition> definitions)
        {
            if (Module != null) return Module.Write(definitions);

            return false;
        }

        /// <summary>
        /// Write Samples to the database
        /// </summary>
        public static bool WriteArchivedSamples(List<Sample> samples)
        {
            if (Module != null) return Module.WriteArchivedSamples(samples);

            return false;
        }

        /// <summary>
        /// Write Samples to the database
        /// </summary>
        public static bool WriteCurrentSamples(List<Sample> samples)
        {
            if (Module != null) return Module.WriteCurrentSamples(samples);

            return false;
        }

        /// <summary>
        /// Write RejectedParts to the database
        /// </summary>
        public static bool Write(List<RejectedPart> parts)
        {
            if (Module != null) return Module.Write(parts);

            return false;
        }

        /// <summary>
        /// Write VerifiedParts to the database
        /// </summary>
        public static bool Write(List<VerifiedPart> parts)
        {
            if (Module != null) return Module.Write(parts);

            return false;
        }

        /// <summary>
        /// Write Statuses to the database
        /// </summary>
        public static bool Write(List<Status> statuses)
        {
            if (Module != null) return Module.Write(statuses);

            return false;
        }

        #endregion

        #region "Delete"

        /// <summary>
        /// Delete RejectedPart from the database
        /// </summary>
        public static bool DeleteRejectedPart(string deviceId, string partId)
        {
            if (Module != null) return Module.DeleteRejectedPart(deviceId, partId);

            return false;
        }

        /// <summary>
        /// Delete VerifiedPart from the database
        /// </summary>
        public static bool DeleteVerifiedPart(string deviceId, string partId)
        {
            if (Module != null) return Module.DeleteVerifiedPart(deviceId, partId);

            return false;
        }

        #endregion

    }
}

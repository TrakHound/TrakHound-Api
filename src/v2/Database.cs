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
        private static IDatabaseModule module;


        public static bool Initialize(string configurationPath)
        {
            var assemblyDir = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            var path = Path.Combine(assemblyDir, configurationPath);
            if (!string.IsNullOrEmpty(path))
            {
                log.Info("Reading Database Configuration File From '" + path + "'");

                var modules = FindModules(assemblyDir);
                if (modules != null)
                {
                    foreach (var module in modules)
                    {
                        try
                        {
                            if (module.Initialize(path))
                            {
                                log.Info(module.Name + " Database Module Initialize Successfully");
                                Database.module = module;
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
            if (module != null)
            {
                try
                {
                    module.Close();
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

        /// <summary>
        /// Read all of the Connections available from the DataServer
        /// </summary>
        public static List<ConnectionDefinition> ReadConnections()
        {
            if (module != null) return module.ReadConnections();

            return null;
        }

        /// <summary>
        /// Read the ConnectionDefintion from the database
        /// </summary>
        public static ConnectionDefinition ReadConnection(string deviceId)
        {
            if (module != null) return module.ReadConnection(deviceId);

            return null;
        }

        /// <summary>
        /// Read the most current AgentDefintion from the database
        /// </summary>
        public static AgentDefinition ReadAgent(string deviceId)
        {
            if (module != null) return module.ReadAgent(deviceId);

            return null;
        }

        /// <summary>
        /// Read AssetDefintions from the database
        /// </summary>
        public static List<AssetDefinition> ReadAssets(string deviceId, string assetId, DateTime from, DateTime to, DateTime at, long count)
        {
            if (module != null) return module.ReadAssets(deviceId, assetId, from, to, at, count);

            return null;
        }

        /// <summary>
        /// Read the ComponentDefinitions for the specified Agent Instance Id from the database
        /// </summary>
        public static List<ComponentDefinition> ReadComponents(string deviceId, long agentInstanceId)
        {
            if (module != null) return module.ReadComponents(deviceId, agentInstanceId);

            return null;
        }

        /// <summary>
        /// Read the DataItemDefinitions for the specified Agent Instance Id from the database
        /// </summary>
        public static List<DataItemDefinition> ReadDataItems(string deviceId, long agentInstanceId)
        {
            if (module != null) return module.ReadDataItems(deviceId, agentInstanceId);

            return null;
        }

        /// <summary>
        /// Read the DeviceDefintion for the specified Agent Instance Id from the database
        /// </summary>
        public static DeviceDefinition ReadDevice(string deviceId, long agentInstanceId)
        {
            if (module != null) return module.ReadDevice(deviceId, agentInstanceId);

            return null;
        }

        /// <summary>
        /// Read Samples from the database
        /// </summary>
        public static List<Sample> ReadSamples(string[] dataItemIds, string deviceId, DateTime from, DateTime to, DateTime at, long count)
        {
            if (module != null) return module.ReadSamples(dataItemIds, deviceId, from, to, at, count);

            return null;
        }

        /// <summary>
        /// Read RejectedParts from the database
        /// </summary>
        public static List<RejectedPart> ReadRejectedParts(string deviceId, string[] partIds, DateTime from, DateTime to, DateTime at)
        {
            if (module != null) return module.ReadRejectedParts(deviceId, partIds, from, to, at);

            return null;
        }

        /// <summary>
        /// Read VerifiedParts from the database
        /// </summary>
        public static List<VerifiedPart> ReadVerifiedParts(string deviceId, string[] partIds, DateTime from, DateTime to, DateTime at)
        {
            if (module != null) return module.ReadVerifiedParts(deviceId, partIds, from, to, at);

            return null;
        }

        /// <summary>
        /// Read the Status from the database
        /// </summary>
        public static Status ReadStatus(string deviceId)
        {
            if (module != null) return module.ReadStatus(deviceId);

            return null;
        }

        #endregion

        #region "Write"

        /// <summary>
        /// Write ConnectionDefinitions to the database
        /// </summary>
        public static bool Write(List<ConnectionDefinitionData> definitions)
        {
            if (module != null) return module.Write(definitions);

            return false;
        }

        /// <summary>
        /// Write AgentDefintions to the database
        /// </summary>
        public static bool Write(List<AgentDefinitionData> definitions)
        {
            if (module != null) return module.Write(definitions);

            return false;
        }

        /// <summary>
        /// Write AssetDefintions to the database
        /// </summary>
        public static bool Write(List<AssetDefinitionData> definitions)
        {
            if (module != null) return module.Write(definitions);

            return false;
        }

        /// <summary>
        /// Write ComponentDefintions to the database
        /// </summary>
        public static bool Write(List<ComponentDefinitionData> definitions)
        {
            if (module != null) return module.Write(definitions);

            return false;
        }

        /// <summary>
        /// Write DataItemDefintions to the database
        /// </summary>
        public static bool Write(List<DataItemDefinitionData> definitions)
        {
            if (module != null) return module.Write(definitions);

            return false;
        }

        /// <summary>
        /// Write DeviceDefintions to the database
        /// </summary>
        public static bool Write(List<DeviceDefinitionData> definitions)
        {
            if (module != null) return module.Write(definitions);

            return false;
        }

        /// <summary>
        /// Write Samples to the database
        /// </summary>
        public static bool Write(List<SampleData> samples)
        {
            if (module != null) return module.Write(samples);

            return false;
        }

        /// <summary>
        /// Write RejectedParts to the database
        /// </summary>
        public static bool Write(List<RejectedPart> parts)
        {
            if (module != null) return module.Write(parts);

            return false;
        }

        /// <summary>
        /// Write VerifiedParts to the database
        /// </summary>
        public static bool Write(List<VerifiedPart> parts)
        {
            if (module != null) return module.Write(parts);

            return false;
        }

        /// <summary>
        /// Write Statuses to the database
        /// </summary>
        public static bool Write(List<StatusData> statuses)
        {
            if (module != null) return module.Write(statuses);

            return false;
        }

        #endregion

        #region "Delete"

        /// <summary>
        /// Delete RejectedPart from the database
        /// </summary>
        public static bool DeleteRejectedPart(string deviceId, string partId)
        {
            if (module != null) return module.DeleteRejectedPart(deviceId, partId);

            return false;
        }

        /// <summary>
        /// Delete VerifiedPart from the database
        /// </summary>
        public static bool DeleteVerifiedPart(string deviceId, string partId)
        {
            if (module != null) return module.DeleteVerifiedPart(deviceId, partId);

            return false;
        }

        #endregion

    }
}

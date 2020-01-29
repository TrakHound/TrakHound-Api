// Copyright (c) 2020 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using TrakHound.Api.v2.Configurations;
using TrakHound.Api.v2.Data;

namespace TrakHound.Api.v2
{
    public static class Database
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public static bool Verbose = true;

        /// <summary>
        /// The currently loaded IDatabaseModule
        /// </summary>
        public static IDatabaseModule Module;

        /// <summary>
        /// The currently loaded DatabaseConfiguration
        /// </summary>
        public static DatabaseConfiguration Configuration { get; set; }

        public static string ConnectionString { get; set; }

        public static bool Initialize(DatabaseConfiguration config)
        {
            var assemblyDir = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            if (config != null && !string.IsNullOrEmpty(assemblyDir))
            {
                if (Verbose) log.Info("Reading Database Modules From '" + assemblyDir + "'");

                var modules = FindModules(assemblyDir);
                if (modules != null)
                {
                    foreach (var module in modules)
                    {
                        try
                        {
                            if (module.Initialize(config))
                            {
                                if (Verbose) log.Info(module.Name + " Database Module Initialize Successfully");
                                Module = module;
                                Configuration = config;
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

        #region "Settings"

        public static bool UpdateSetting(string name, string value)
        {
            if (Module != null) return Module.UpdateSetting(name, value);

            return false;
        }

        public static string ReadSetting(string name)
        {
            if (Module != null) return Module.ReadSetting(name);

            return null;
        }

        #endregion

        #region "Read"

        public static bool ExecuteQuery(string query)
        {
            if (Module != null) return Module.ExecuteQuery(query);

            return false;
        }

        public static T Read<T>(string query)
        {
            if (Module != null) return Module.Read<T>(query);

            return default(T);
        }

        public static List<T> ReadList<T>(string query)
        {
            if (Module != null) return Module.ReadList<T>(query);

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
        /// Read the Status from the database
        /// </summary>
        public static Status ReadStatus(string deviceId)
        {
            if (Module != null) return Module.ReadStatus(deviceId);

            return null;
        }

        #endregion

    }
}

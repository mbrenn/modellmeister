﻿using ModellMeister.Logic.Reporting;
using ModellMeister.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Runner
{
    /// <summary>
    /// Stores the instance, which is controlling the server. 
    /// </summary>
    public class SimulationClient : MarshalByRefObject
    {
        /// <summary>
        /// Gets or sets the simulation result
        /// </summary>
        public SimulationResult SimulationResult
        {
            get;
            set;
        }

        public SimulationSettings SimulationSettings
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the SimulationClient class. 
        /// </summary>
        /// <param name="result"></param>
        public SimulationClient(SimulationSettings settings)
        {
            this.SimulationResult = new SimulationResult();
            this.SimulationSettings = settings;
        }

        public async Task RunSimulationInAppDomain(string workspacePath, string dllName)
        {
            var setup = new AppDomainSetup()
            {
                ApplicationBase = workspacePath,
                PrivateBinPath = workspacePath,
                ConfigurationFile = null
            };

            var appDomain = AppDomain.CreateDomain("Runner", null, setup);
            var type = (SimulationServer)appDomain.CreateInstanceAndUnwrap(
                "ModellMeister",
                typeof(SimulationServer).FullName);

            type.Settings = this.SimulationSettings;
            type.Client = this;
            await Task.Run(() => { type.LoadAndStartFromLibrarySync(dllName); });

            AppDomain.Unload(appDomain);
        }

        /// <summary>
        /// Called 
        /// </summary>
        public void Step()
        {
        }

        /// <summary>
        /// Adds a point into the database
        /// </summary>
        /// <param name="absoluteTime">Absolute time to be added</param>
        /// <param name="values">Values to be added</param>
        public void AddResult(StepInfo info, object[] values)
        {
            this.SimulationResult.Result.Add(
                new StateAtTime(info.AbsoluteTime, values));
        }
    }
}

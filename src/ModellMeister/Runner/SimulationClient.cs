using ModellMeister.Logic.Reporting;
using ModellMeister.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
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
        /// Gets or sets the server for the simulation. The server is executing the simulation
        /// </summary>
        public SimulationServer Server
        {
            get;
            set;
        }

        /// <summary>
        /// This event is thrown, when a simualtion step has been performed
        /// </summary>
        public event EventHandler Stepped;

        /// <summary>
        /// Initializes a new instance of the SimulationClient class. 
        /// </summary>
        /// <param name="result"></param>
        public SimulationClient(SimulationSettings settings)
        {
            this.SimulationResult = new SimulationResult();
            this.SimulationSettings = settings;
        }

        /// <summary>
        /// Runs the simulation in the application domain, which is deleted after the simulation
        /// </summary>
        /// <param name="workspacePath">Path to the worksapce</param>
        /// <param name="dllName">Name of the dll</param>
        /// <returns>The task which is used to execute the simulationserver</returns>
        public async Task RunSimulationInAppDomain(string pathToAssembly)
        {
            var binPath = Path.GetDirectoryName(pathToAssembly);
            var dllName = Path.GetFileName(pathToAssembly);

            var oldCurrentDirectory = Environment.CurrentDirectory;

            Environment.CurrentDirectory = binPath;
            var setup = new AppDomainSetup()
            {
                ApplicationBase = binPath,
                PrivateBinPath = binPath,
                ConfigurationFile = null
            };

            var appDomain = AppDomain.CreateDomain("Runner", null, setup);
            this.Server = (SimulationServer)appDomain.CreateInstanceAndUnwrap(
                "ModellMeister",
                typeof(SimulationServer).FullName);

            this.Server.Settings = this.SimulationSettings;
            this.Server.Client = this;
            await Task.Run(() => { this.Server.LoadAndStartFromLibrarySync(dllName); });

            this.Server = null;

            AppDomain.Unload(appDomain);

            Environment.CurrentDirectory = oldCurrentDirectory;
        }

        /// <summary>
        /// Called 
        /// </summary>
        public void Step()
        {
            var ev = this.Stepped;
            if (ev != null)
            {
                ev(this, EventArgs.Empty);
            }
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

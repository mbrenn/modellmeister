using ModellMeister.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Runner
{
    /// <summary>
    /// Executes the simulation
    /// </summary>
    public class Simulation : MarshalByRefObject, IDebugEnvironment
    {
        /// <summary>
        /// Stores the modeltype
        /// </summary>
        private IModelType modelType;

        /// <summary>
        /// Gets or sets the settings
        /// </summary>
        public SimulationSettings Settings
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the Simulation. 
        /// This constructor is necessary, since the simulation is started 
        /// via remote constructor in other AppDomain.
        /// </summary>
        public Simulation()
        {
        }

        public Simulation(SimulationSettings settings)
        {
            this.Settings = settings;
        }

        /// <summary>
        /// Loads the library and starts it
        /// </summary>
        /// <param name="pathToLibrary">Library to be started</param>
        public async Task<SimulationResult> LoadAndStartFromLibrary(
            string pathToLibrary)
        {
            this.LoadFromLibrary(pathToLibrary);
            return await this.StartSimulation();
        }

        /// <summary>
        /// Loads the library and starts it
        /// </summary>
        /// <param name="pathToLibrary">Library to be started</param>
        public SimulationResult LoadAndStartFromLibrarySync(
            string pathToLibrary)
        {
            return this.LoadAndStartFromLibrary(pathToLibrary).Result;
        }

        /// <summary>
        /// Loads the object from a model library
        /// </summary>
        /// <param name="pathToLibrary">Path to library from which the model shall be loaded</param>
        /// <returns>Model type being loaded</returns>
        private IModelType LoadFromLibrary(string pathToLibrary)
        {
            var absolutePath = Path.Combine(Environment.CurrentDirectory, pathToLibrary);

            if (!File.Exists(absolutePath))
            {
                throw new InvalidOperationException("Library does not exist: " + absolutePath);
            }

            Console.WriteLine("- Loading library: " + Path.GetFileName(absolutePath));

            var loadedAssembly = Assembly.LoadFile(absolutePath);
            var types = loadedAssembly.GetTypes().Where(
                x => x.GetCustomAttribute(typeof(RootModelAttribute)) != null)
                .ToList();

            if (types.Count == 0)
            {
                throw new InvalidOperationException("None of the types have a RootModelAttribute");
            }
            else if (types.Count > 1)
            {
                throw new InvalidOperationException("There is more than one type which has a RootModelAttribute");
            }
            else
            {
                // Ok, got it... 
                var foundType = types.First();
                this.modelType = Activator.CreateInstance(foundType) as IModelType;

                if (this.modelType == null)
                {
                    throw new InvalidOperationException("Instance for type '" + foundType + "' could not be created. Is it of type IModelType?");
                }

                return this.modelType;
            }
        }

        /// <summary>
        /// Starts the simulation
        /// </summary>
        public async Task<SimulationResult> StartSimulation()
        {
            return await Task.Run(() =>
            {
                var results = new List<StateAtTime>();

                if (this.Settings.TimeInterval.TotalSeconds <= 0)
                {
                    throw new InvalidOperationException("Time Interval is negative or null. Not allowed");
                }

                if (this.modelType == null)
                {
                    throw new InvalidOperationException("No model loaded");
                }

                this.modelType.Init();

                var result = new SimulationResult();

                var step = new StepInfoForSimulation(this, result);
                step.TimeInterval = this.Settings.TimeInterval;
                step.Debug = this;

                for (var currentTime = 0.0;
                        currentTime < this.Settings.SimulationTime.TotalSeconds;
                        currentTime += this.Settings.TimeInterval.TotalSeconds)
                {
                    step.AbsoluteTime = TimeSpan.FromSeconds(currentTime);

                    this.modelType.Execute(step);
                }

                return result;
            });
        }

        /// <summary>
        /// Adds a point into the database
        /// </summary>
        /// <param name="absoluteTime">Absolute time to be added</param>
        /// <param name="values">Values to be added</param>
        public void AddResult(StepInfo info, object[] values)
        {
            (info as StepInfoForSimulation).SimulationResult.Result.Add(
                new StateAtTime(info.AbsoluteTime, values));
        }
    }
}

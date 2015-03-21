using ModellMeister.Logic.Reporting;
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
    public class SimulationServer : MarshalByRefObject
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
        /// Gets or sets the settings
        /// </summary>
        public SimulationClient Client
        {
            get;
            set;
        }

        public WatchList WatchList
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the Simulation. 
        /// This constructor is necessary, since the simulation is started 
        /// via remote constructor in other AppDomain.
        /// </summary>
        public SimulationServer()
        {
            this.WatchList = new WatchList();
        }

        /// <summary>
        /// Loads the library and starts it
        /// </summary>
        /// <param name="pathToLibrary">Library to be started</param>
        public async Task<SimulationResult> LoadAndStartFromLibrary(
            string pathToLibrary)
        {
            if (this.Client == null)
            {
                throw new InvalidOperationException("this.Client is not set for server");
            }

            this.LoadFromLibrary(pathToLibrary);
            return await this.StartSimulation();
        }

        /// <summary>
        /// Loads the library and starts it
        /// </summary>
        /// <param name="pathToLibrary">Library to be started</param>
        public void LoadAndStartFromLibrarySync(
            string pathToLibrary)
        {
            this.LoadAndStartFromLibrary(pathToLibrary).Wait();
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
            if (this.Client == null)
            {
                throw new InvalidOperationException("this.Client is not set for server");
            }

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

                var step = new StepInfoForSimulation(this);
                step.TimeInterval = this.Settings.TimeInterval;

                for (var currentTime = 0.0;
                        currentTime < this.Settings.SimulationTime.TotalSeconds;
                        currentTime += this.Settings.TimeInterval.TotalSeconds)
                {
                    step.AbsoluteTime = TimeSpan.FromSeconds(currentTime);

                    this.modelType.Execute(step);

                    this.Client.Step();
                    this.AddByWatchList(step);
                }

                return result;
            });
        }

        /// <summary>
        /// Adds a channel to the result
        /// </summary>
        /// <param name="channelInformation"></param>
        public void AddChannel(WatchListItem channelInformation)
        {
            this.WatchList.Add(channelInformation);
        }

        /// <summary>
        /// Adds the content to the result
        /// </summary>
        public void AddByWatchList(StepInfo info)
        {
            if (this.WatchList.Items.Count == 0)
            {
                // No items on watchlist, no return
                return;
            }

            var maxValue = this.WatchList.Items.Max(x => x.Index);
            var values = new object[maxValue + 1];

            foreach (var item in this.WatchList.Items)
            {
                values[item.Index] = item.ModelType.GetPortValue(item.PortName);
            }
        }

        /// <summary>
        /// Gets a value from a specific port
        /// </summary>
        /// <param name="name">Name of the port to be queried. Block.InnerBlock.PortName</param>
        /// <returns>The value of the port</returns>
        public object GetPortValue(string name)
        {
            var nameParts = name.Split(new[] { '.' });

            var currentModelType = this.modelType;
            for (var n = 0; n < (nameParts.Length - 1); n++)
            {
                var compositeModelType = currentModelType as ICompositeModelType;
                currentModelType = compositeModelType.GetBlock(nameParts[n]) as IModelType;
            }

            return currentModelType.GetPortValue(nameParts.LastOrDefault());
        }
    }
}

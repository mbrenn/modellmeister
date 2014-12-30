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
    public class Simulation
    {
        /// <summary>
        /// Stores the modeltype
        /// </summary>
        private IModelType modelType;

        private SimulationSettings settings;

        public Simulation(SimulationSettings settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Loads the library and starts it
        /// </summary>
        /// <param name="pathToLibrary">Library to be started</param>
        public void LoadAndStartFromLibrary(
            string pathToLibrary)
        {
            this.LoadFromLibrary(pathToLibrary);
            this.StartSimulation();
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
        public void StartSimulation()
        {
            if (this.settings.TimeInterval.TotalSeconds <= 0)
            {
                throw new InvalidOperationException("Time Interval is negative or null. Not allowed");
            }

            if (this.modelType == null)
            {
                throw new InvalidOperationException("No model loaded");
            }

            this.modelType.Init();

            for (var currentTime = 0.0;
                currentTime < this.settings.SimulationTime.TotalSeconds;
                currentTime += this.settings.TimeInterval.TotalSeconds)
            {
                this.modelType.Execute();
            }
        }
    }
}

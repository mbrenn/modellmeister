using ModellMeister.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbsim
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("BurnSystems Model Simulator");

            if (args.Length != 1)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("mbsim.exe model.dll");
                Console.WriteLine();
                Console.WriteLine("Loads the model in model.dll and executes it");
            }

            // Simulationtime is 10 seconds
            var simulationSettings = new SimulationSettings();
            simulationSettings.SimulationTime= TimeSpan.FromSeconds(10);
            simulationSettings.TimeInterval = TimeSpan.FromSeconds(0.1);

            var simulation = new Simulation(simulationSettings);
            simulation.LoadAndStartFromLibrary(
                args[0]).Wait();
        }
    }
}

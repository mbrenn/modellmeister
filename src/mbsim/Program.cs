using BurnSystems.CommandLine;
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
            var result = Parser.ParseIntoOrShowUsage<ProgramArguments>(args);

            if (result != null)
            {
                Console.WriteLine("BurnSystems Model Simulator");

                // Simulationtime is 10 seconds
                var simulationSettings = new SimulationSettings();
                simulationSettings.SimulationTime = TimeSpan.FromSeconds(result.SimulationTime);
                simulationSettings.TimeInterval = TimeSpan.FromSeconds(result.SimulationInterval);

                var client = new SimulationClient(simulationSettings);

                var simulation = new SimulationServer();
                simulation.Settings = simulationSettings;
                simulation.Client = client;
                simulation.LoadAndStartFromLibrary(
                    result.File).Wait();
            }
        }
    }
}

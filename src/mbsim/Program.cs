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
            var result = CommandLine.Parser.Default.ParseArguments<ProgramArguments>(args);

            if (!result.Errors.Any())
            {
                Console.WriteLine("BurnSystems Model Simulator");

                // Simulationtime is 10 seconds
                var simulationSettings = new SimulationSettings();
                simulationSettings.SimulationTime = TimeSpan.FromSeconds(result.Value.SimulationTime);
                simulationSettings.TimeInterval = TimeSpan.FromSeconds(result.Value.SimulationInterval);

                var simulation = new Simulation(simulationSettings);
                simulation.LoadAndStartFromLibrary(
                    result.Value.File).Wait();
            }
        }
    }
}

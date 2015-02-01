using ModellMeister.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Runner
{
    public class StepInfoForSimulation : StepInfo
    {
        public Simulation Simulation
        {
            get;
            set;
        }

        public SimulationResult SimulationResult
        {
            get;
            set;
        }

        public StepInfoForSimulation(Simulation simulation, SimulationResult result)
        {
            this.Simulation = simulation;
            this.SimulationResult = result;
        }
    }
}

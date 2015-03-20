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
        public SimulationServer Simulation
        {
            get;
            set;
        }

        public StepInfoForSimulation(SimulationServer simulation)
        {
            this.Simulation = simulation;
        }
    }
}

using ModellMeister.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Runner
{
    [Serializable]
    public class StepInfoForSimulation : StepInfo
    {
        public SimulationServer Simulation
        {
            get { return this.Server as SimulationServer; }
            set { this.Server = value; }
        }

        public StepInfoForSimulation(SimulationServer simulation)
        {
            this.Simulation = simulation;
        }
    }
}

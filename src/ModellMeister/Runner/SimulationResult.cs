using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Runner
{
    [Serializable]
    public class SimulationResult
    {
        /// <summary>
        /// Gets or sets the results
        /// </summary>
        public IList<StateAtTime> Result
        {
            get;
            set;
        }

        public SimulationResult()
        {
            this.Result = new List<StateAtTime>();
        }
    }
}

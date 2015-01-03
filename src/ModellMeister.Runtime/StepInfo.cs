using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Runtime
{
    public class StepInfo
    {
        /// <summary>
        /// Gets or sets the absolute time since the start of the simulation
        /// </summary>
        public TimeSpan AbsoluteTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the timeinterval for each simulation step
        /// </summary>
        public TimeSpan TimeInterval
        {
            get;
            set;
        }

        public IDebugEnvironment Debug
        {
            get;
            set;
        }
    }
}

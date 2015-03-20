using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Runner
{
    [Serializable]
    public class SimulationSettings
    {
        public TimeSpan SimulationTime
        {
            get;
            set;
        }

        public TimeSpan TimeInterval
        {
            get;
            set;
        }

        /// <summary>
        /// The simulation will be run with real time, not with
        /// maximum speed. To be used for simulation with manipulation
        /// </summary>
        public bool Synchronous
        {
            get;
            set;
        }

        public double SimulationTimeS
        {
            get { return this.SimulationTime.TotalSeconds; }
            set { this.SimulationTime = TimeSpan.FromSeconds(value); }
        }

        public double TimeIntervalMS
        {
            get { return this.TimeInterval.TotalSeconds * 1000.0; }
            set { this.TimeInterval = TimeSpan.FromSeconds(value / 1000.0); }
        }

        public double TimeIntervalS
        {
            get { return this.TimeInterval.TotalSeconds; }
            set { this.TimeInterval = TimeSpan.FromSeconds(value); }
        }

        public SimulationSettings()
        {
            this.SimulationTime = TimeSpan.FromSeconds(10);
            this.TimeInterval = TimeSpan.FromSeconds(0.1);
        }
    }
}

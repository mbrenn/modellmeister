using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Runner
{
    /// <summary>
    /// Stores the information for a given time span
    /// </summary>
    [Serializable]
    public class StateAtTime
    {
        public TimeSpan AbsoluteTime
        {
            get;
            set;
        }

        public object[] Values
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the StateAtTime class
        /// </summary>
        /// <param name="absoluteTime">Absolute time to be used</param>
        /// <param name="values">Values to be stored</param>
        public StateAtTime(TimeSpan absoluteTime, object[] values)
        {
            this.AbsoluteTime = absoluteTime;
            this.Values = values;
        }
    }
}

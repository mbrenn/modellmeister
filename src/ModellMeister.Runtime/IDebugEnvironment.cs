using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Runtime
{
    public interface IDebugEnvironment
    {
        /// <summary>
        /// Adds some results to the debug interface, when available
        /// </summary>
        /// <param name="values">Value to be added</param>
        void AddResult(StepInfo info, object[] values);
    }
}

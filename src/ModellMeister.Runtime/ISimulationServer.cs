using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Runtime
{
    /// <summary>
    /// Stores the interface to simulation of the server
    /// </summary>
    public interface ISimulationServer
    {
        /// <summary>
        /// Adds a watch
        /// </summary>
        /// <param name="type">Type to be added to watch</param>
        /// <param name="portName">Name of the port</param>
        void AddWatch(IModelType type, string portName);
    }
}

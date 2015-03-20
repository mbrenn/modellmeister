using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Runtime
{
    /// <summary>
    /// Defines the possible port types
    /// </summary>
    public enum PortType
    {
        /// <summary>
        /// Input port, which can be wired to output. 
        /// </summary>
        Input, 

        /// <summary>
        /// Output port, which sends information to an input port
        /// </summary>
        Output,

        /// <summary>
        /// A property, which has a fixed value during simulation
        /// </summary>
        Property
    }
}

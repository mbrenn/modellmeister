using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Model
{
    /// <summary>
    /// Defines a wire between two points
    /// </summary>
    public class Wire
    {
        /// <summary>
        /// Gets or sets the input of the wire
        /// </summary>
        public Port InputOfWire
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the output of the wire
        /// </summary>
        public Port OutputOfWire
        {
            get;
            set;
        }
    }
}

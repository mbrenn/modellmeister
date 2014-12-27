using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Model
{
    /// <summary>
    /// Defines an entity which has a name and contains several ports
    /// </summary>
    public class EntityWithPorts : Entity
    {
        private List<Port> inputs = new List<Port>();

        private List<Port> outputs = new List<Port>();

        /// <summary>
        /// Gets the input ports
        /// </summary>
        public List<Port> Inputs
        {
            get { return this.inputs; }
        }

        /// <summary>
        /// Gets the output ports
        /// </summary>
        public List<Port> Outputs
        {
            get { return this.outputs; }
        }
    }
}

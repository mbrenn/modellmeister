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
    public abstract class EntityWithPorts : Entity
    {
        private List<ModelPort> inputs = new List<ModelPort>();

        private List<ModelPort> outputs = new List<ModelPort>();

        /// <summary>
        /// Gets the input ports
        /// </summary>
        public List<ModelPort> Inputs
        {
            get { return this.inputs; }
        }

        /// <summary>
        /// Gets the output ports
        /// </summary>
        public List<ModelPort> Outputs
        {
            get { return this.outputs; }
        }
    }
}

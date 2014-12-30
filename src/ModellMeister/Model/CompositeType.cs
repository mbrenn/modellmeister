using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Model
{
    /// <summary>
    /// STre
    /// </summary>
    public class CompositeType : ModelType
    {
        /// <summary>
        /// Stores the list of types
        /// </summary>
        private List<ModelType> types = new List<ModelType>();

        private List<ModelBlock> blocks = new List<ModelBlock>();

        private List<Wire> wires = new List<Wire>();

        /// <summary>
        /// Gets the types
        /// </summary>
        public List<ModelType> Types
        {
            get { return this.types; }
        }

        /// <summary>
        /// Gets the types
        /// </summary>
        public List<ModelBlock> Blocks
        {
            get { return this.blocks; }
        }

        /// <summary>
        /// Gets the types
        /// </summary>
        public List<Wire> Wires
        {
            get { return this.wires; }
        }
    }
}

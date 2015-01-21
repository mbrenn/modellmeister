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
    public class ModelCompositeType : ModelType
    {
        /// <summary>
        /// Stores the list of types
        /// </summary>
        private List<ModelType> types = new List<ModelType>();

        private List<ModelBlock> blocks = new List<ModelBlock>();

        private List<ModelWire> wires = new List<ModelWire>();

        private List<ModelWire> feedbackWires = new List<ModelWire>();

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
        public List<ModelWire> Wires
        {
            get { return this.wires; }
        }

        /// <summary>
        /// Gets the types
        /// </summary>
        public List<ModelWire> FeedbackWires
        {
            get { return this.feedbackWires; }
        }
    }
}

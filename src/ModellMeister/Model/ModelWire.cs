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
    public class ModelWire
    {
        /// <summary>
        /// Gets or sets the input of the wire
        /// </summary>
        public ModelPort InputOfWire
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the output of the wire
        /// </summary>
        public ModelPort OutputOfWire
        {
            get;
            set;
        }

        public ModelWire()
        {
        }

        public ModelWire(ModelPort inputOfWire, ModelPort outputOfWire)
        {
            this.InputOfWire = inputOfWire;
            this.OutputOfWire = outputOfWire;
        }
        public override string ToString()
        {
            if (this.InputOfWire == null || this.OutputOfWire == null)
            {
                return base.ToString();
            }

            return string.Format("Wire: " + this.InputOfWire.Name + " -> " + this.OutputOfWire.Name);
        }
    }
}

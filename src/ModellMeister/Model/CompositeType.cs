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
    public class CompositeType : EntityWithPorts
    {
        /// <summary>
        /// Stores the list of types
        /// </summary>
        private List<ModelType> types = new List<ModelType>();

        /// <summary>
        /// Gets the types
        /// </summary>
        public List<ModelType> Types
        {
            get { return this.types; }
        }
    }
}

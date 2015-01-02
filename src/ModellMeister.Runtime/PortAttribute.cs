using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Runtime
{
    /// <summary>
    /// Defines the attribute for all ports, which describes
    /// the port better
    /// </summary>
    public class PortAttribute : Attribute
    {
        public PortAttribute(PortType type)
        {
            this.PortType = type;
        }

        /// <summary>
        /// Gets or sets the type of the property
        /// </summary>
        public PortType PortType
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Model
{
    /// <summary>
    /// Defines a single port
    /// </summary>
    public class Port : Entity
    {
        /// <summary>
        /// Gets or sets the data type
        /// </summary>
        public DataType DataType
        {
            get;
            set;
        }

        /// <summary>
        /// Clones the port
        /// </summary>
        /// <returns></returns>
        public Port Clone()
        {
            var newPort = new Port();
            newPort.Name = this.Name;
            newPort.DataType = this.DataType;
            return newPort;
        }
    }
}

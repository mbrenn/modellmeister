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
    public class ModelPort : Entity
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
        /// Gets or sets the default value
        /// </summary>
        public object DefaultValue
        {
            get;
            set;
        }

        /// <summary>
        /// Clones the port
        /// </summary>
        /// <returns></returns>
        public ModelPort Clone()
        {
            var newPort = new ModelPort();
            newPort.Name = this.Name;
            newPort.DataType = this.DataType;
            newPort.DefaultValue = this.DefaultValue;
            return newPort;
        }
    }
}

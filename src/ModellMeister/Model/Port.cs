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
    }
}

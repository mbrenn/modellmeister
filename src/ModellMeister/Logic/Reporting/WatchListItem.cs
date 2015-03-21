using ModellMeister.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Logic.Reporting
{
    public class WatchListItem
    {
        /// <summary>
        /// Gets or sets the index
        /// </summary>
        public int Index
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the channel
        /// </summary>
        public IModelType ModelType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the channel
        /// </summary>
        public string PortName
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the WatchListItem class
        /// </summary>
        public WatchListItem()
        {
            this.Index = -1;
        }
    }
}

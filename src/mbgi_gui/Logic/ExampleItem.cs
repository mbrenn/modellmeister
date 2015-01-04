using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbgi_gui.Logic
{
    public class ExampleItem
    {
        /// <summary>
        /// Gets or sets the name of the exampe
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the exampe
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mbgi file
        /// </summary>
        public string MbgiFile
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the cs file
        /// </summary>
        public string CsFile
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.Description;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbgi_gui.Models
{
    public class GuiSettings
    {
        public string WorkspacePath
        {
            get;
            set;
        }

        public string Filename
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current file path for the MBGI file. 
        /// This will be used to store the file, when necessary
        /// </summary>
        public string CurrentMbgiFilePath
        {
            get;
            set;
        }
    }
}

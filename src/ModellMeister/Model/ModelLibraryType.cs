using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Model
{
    public class ModelLibraryType : ModelType
    {
        /// <summary>
        /// Defines the DotNetType for the given native Type
        /// </summary>
        public Type DotNetType
        {
            get;
            set;
        }
    }
}

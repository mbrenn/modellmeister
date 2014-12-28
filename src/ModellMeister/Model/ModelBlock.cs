using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Model
{
    public class ModelBlock : EntityWithPorts
    {
        /// <summary>
        /// Gets or sets the model type
        /// </summary>
        public ModelType Type
        {
            get;
            set;
        }
    }
}

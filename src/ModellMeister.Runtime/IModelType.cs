using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Runtime
{
    public interface IModelType
    {
        /// <summary>
        /// Initializes a modeltype
        /// </summary>
        void Init(StepInfo info);
        
        /// <summary>
        /// Executes the object
        /// </summary>
        void Execute(StepInfo info);

        /// <summary>
        /// Gets a certain port value
        /// </summary>
        /// <param name="name">Name of the port</param>
        /// <returns>The given value</returns>
        object GetPortValue(string name);
    }
}

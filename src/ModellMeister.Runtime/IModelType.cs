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
        void Init();
        
        /// <summary>
        /// Executes the object
        /// </summary>
        void Execute(StepInfo info);
    }
}

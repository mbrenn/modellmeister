using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Runner
{
    [Serializable]
    public class SimulationResult
    {
        public IList<object[]> Result
        {
            get;
            set;
        }
    }
}

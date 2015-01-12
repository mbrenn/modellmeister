using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Library.Helper 
{
    public partial class CurrentTime
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = info.AbsoluteTime.TotalSeconds;
        }
    }

    public partial class ExecutionAbort
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            if (this.Condition)
            {
                throw new InvalidOperationException();
            }
        }
    }
}

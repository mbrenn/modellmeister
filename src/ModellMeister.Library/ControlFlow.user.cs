using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Library.ControlFlow
{
    public partial class Switch
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = this.Condition ? this.TrueValue : this.FalseValue;
        }
    }

}

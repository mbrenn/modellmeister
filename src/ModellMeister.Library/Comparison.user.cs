using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Library.Comparison
{
    public partial class Equal
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Result = this.Input1 == this.Input2;
        }
    }

    public partial class NotEqual
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Result = this.Input1 != this.Input2;
        }
    }

    public partial class GreaterThan
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Result = this.Input1 > this.Input2;
        }
    }

    public partial class SmallerThan
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Result = this.Input1 < this.Input2;
        }
    }

    public partial class GreaterOrEqualThan
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Result = this.Input1 >= this.Input2;
        }
    }

    public partial class SmallerOrEqualThan
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Result = this.Input1 <= this.Input2;
        }
    }
}

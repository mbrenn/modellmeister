using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Library.Logic 
{
    public partial class And
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = this.Input1 && this.Input2;
        }
    }

    public partial class Or
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = this.Input1 || this.Input2;
        }
    }
    public partial class Xor
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = this.Input1 ^ this.Input2;
        }
    }
    public partial class Not
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = !this.Input;
        }
    }

}

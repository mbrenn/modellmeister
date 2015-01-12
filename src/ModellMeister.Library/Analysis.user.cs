using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Library.Analysis
{
    public partial class Integral
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output += this.Input * info.TimeInterval.TotalSeconds;
        }
    }

    public partial class Derivative
    {
        private double lastValue = double.NaN;
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            if (Double.IsNaN(lastValue))
            {
                // First iteration is always 0.0
                this.Output = 0.0;
            }
            else
            {
                this.Output = (this.Input - lastValue) / info.AbsoluteTime.TotalSeconds;
            }

            lastValue = this.Input;
        }
    }
}

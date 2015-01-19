using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Library.Source
{
    public partial class Constant
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = this.Input;
        }
    }

    public partial class Sine
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = Math.Sin(
                info.AbsoluteTime.TotalSeconds * this.Frequency * 2 * Math.PI + this.Phase);
        }
    }

    public partial class Triangle
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            var currentTime = info.AbsoluteTime.TotalSeconds + this.Phase;
            currentTime *= this.Frequency;

            var rest = (currentTime + 0.25) % 1;
            if (rest > 0.5)
            {
                this.Output = (1 - rest) * 4 - 1;
            }
            else
            {
                this.Output = rest * 4 - 1;
            }
        }
    }

    public partial class Sawtooth
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            var currentTime = info.AbsoluteTime.TotalSeconds + this.Phase;
            currentTime *= this.Frequency;

            var rest = (currentTime + 0.5)% 1;
            this.Output = Math.Abs(rest) * 2 - 1;
        }
    }
}

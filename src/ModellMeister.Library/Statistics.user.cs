using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Library.Statistics
{
    public partial class Average
    {
        private int samples;

        private double totalSum;

        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.samples++;
            this.totalSum += this.Input;

            this.Output = this.totalSum / this.samples;
        }
    }

    public partial class Variance
    {
        private int samples;

        private double mean;

        private double M2;

        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            // Uses 
            // http://en.wikipedia.org/wiki/Algorithms_for_calculating_variance#Incremental_algorithm
            this.samples++;
            var delta = this.Input - this.mean;
            this.mean += delta / this.samples;
            this.M2 += delta * (this.Input - this.mean);

            if (this.samples < 2)
            {
                this.Output = 0.0;
            }
            else
            {
                this.Output = this.M2 / (this.samples - 1);
            }
        }
    }
}

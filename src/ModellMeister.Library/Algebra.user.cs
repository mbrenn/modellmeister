using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Library.Algebra
{
    public partial class Addition
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Sum = this.Summand1 + this.Summand2;
        }
    }

    public partial class Subtraction
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Difference = this.Minuend - this.Subtrahend;
        }
    }

    public partial class Multiplication
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Product = this.Factor1 * this.Factor2;
        }
    }

    public partial class Division
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Quotient = this.Divident - this.Divisor;
        }
    }

    public partial class Square
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = this.Input * this.Input;
        }
    }

    public partial class SquareRoot
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = Math.Sqrt(this.Input);
        }
    }

    public partial class Sine
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = Math.Sin(this.Input);
        }
    }

    public partial class Cosine
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = Math.Cos(this.Input);
        }
    }

    public partial class Tangens
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = Math.Tan(this.Input);
        }
    }

    public partial class Exponentation
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Power = Math.Pow(this.Base, this.Exponent);
        }
    }

    public partial class Reciprocal
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = 1.0 / this.Input;
        }
    }

    public partial class Logarithm
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = Math.Log(this.Antilogarithm, this.Base);
        }
    }
}

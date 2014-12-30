
namespace ModelBased
{
    public partial class Sine
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = System.Math.Sin(info.AbsoluteTime.TotalSeconds);
        }
    }

    public partial class Constant
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = 1;
        }
    }

    public partial class Adder
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Sum = this.Summand1 + this.Summand2;
        }
    }

    public partial class CSVWriter
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            System.Console.WriteLine(this.Input);
        }
    }
}

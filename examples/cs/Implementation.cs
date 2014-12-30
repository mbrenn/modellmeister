
namespace ModelBased
{
    public partial class Sine
    {
        private double time = 0.0;

        partial void DoExecute()
        {
            time += 0.1;
            this.Output = System.Math.Sin(time);
        }
    }

    public partial class Constant
    {
        partial void DoExecute()
        {
            this.Output = 1;
        }
    }

    public partial class Adder
    {
        partial void DoExecute()
        {
            this.Sum = this.Summand1 + this.Summand2;
        }
    }

    public partial class CSVWriter
    {
        partial void DoExecute()
        {
            System.Console.WriteLine(this.Input);
        }
    }
}

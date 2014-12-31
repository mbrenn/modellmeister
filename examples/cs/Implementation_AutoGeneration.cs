
namespace ModelBased
{
    public partial class RandomType
    {
        private System.Random random = new System.Random();

        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {            
            this.Output = this.random.NextDouble();
        }
    }

    public partial class SquareType
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = this.Input * this.Input;
        }
    }

    public partial class ToConsoleType
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            System.Console.WriteLine(this.Input);
        }
    }
}

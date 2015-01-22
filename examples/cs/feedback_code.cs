
namespace ModelBased
{
    public partial class CSVWriter
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            System.Console.WriteLine(this.Input);
        }
    }
}

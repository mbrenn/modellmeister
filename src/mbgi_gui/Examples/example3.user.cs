namespace ModelBased
{
    public partial class Summer
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Sum = this.Summand1 + this.Summand2;
        }
    }

    public partial class ToConsoleType
    {
        partial void DoInit(ModellMeister.Runtime.StepInfo info)
        {
            info.Server.AddWatch(this, "Input");
        }
    }
}

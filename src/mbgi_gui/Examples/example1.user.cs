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
    public partial class ToConsoleType
    {
        partial void DoInit(ModellMeister.Runtime.StepInfo info)
        {
            info.Server.AddWatch(this, "Input");
        }
    }
}

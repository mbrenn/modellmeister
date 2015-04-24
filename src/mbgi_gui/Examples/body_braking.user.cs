namespace ModelBased
{
    public partial class Constant
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = this.Value;
        }
    }
    public partial class Integrator
    {
        private System.Random random = new System.Random();

        partial void DoInit(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = this.Offset;
        }
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = System.Math.Max(0.0, this.Input * info.TimeInterval.TotalSeconds + this.Output);
        }
    }
    public partial class Report
    {
        partial void DoInit(ModellMeister.Runtime.StepInfo info)
        {
            info.Server.AddWatch(this, "Input1");
            info.Server.AddWatch(this, "Input2");
            info.Server.AddWatch(this, "Input3");
        }
    }
}

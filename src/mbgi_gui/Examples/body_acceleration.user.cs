namespace ModelBased
{
    public partial class Constant
    {
        private System.Random random = new System.Random();

        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {            
            this.Output = this.Value;
        }
    }
    public partial class Integrator
    {
        private System.Random random = new System.Random();

        partial void DoInit()
        {
            this.Output = this.Offset;
        }
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = this.Input * info.TimeInterval.TotalSeconds + this.Output;
        }
    }
    public partial class Report
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            info.Debug.AddResult(info, new object[]{this.Input1, this.Input2, this.Input3});
        }
    }
}

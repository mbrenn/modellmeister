﻿namespace ModelBased
{
    public partial class Random
    {
        private System.Random random = new System.Random();

        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {            
            this.Output = this.random.NextDouble();
        }
    }
    public partial class Increment
    {
        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            this.Output = this.Input + 1;
        }
    }
    public partial class ToConsole
    {
        partial void DoInit(ModellMeister.Runtime.StepInfo info)
        {
            info.Server.AddWatch(this, "Input");
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.34014
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ModellMeister.Library.Helper {
    
    
    [ModellMeister.Runtime.RootModelAttribute()]
    public partial class _ : ModellMeister.Runtime.IModelType {
        
        public void Init() {
        }
        
        public void Execute(ModellMeister.Runtime.StepInfo info) {
        }
    }
    
    public partial class CurrentTime : ModellMeister.Runtime.IModelType {
        
        private double _Output;
        
        partial void DoInit();
        
        partial void DoExecute(ModellMeister.Runtime.StepInfo info);
        
        [ModellMeister.Runtime.Port(ModellMeister.Runtime.PortType.Input)]
        public double Output {
            get {
                return this._Output;
            }
            set {
                this._Output = value;
            }
        }
        
        public void Execute(ModellMeister.Runtime.StepInfo info) {
            this.DoExecute(info);
        }
        
        public void Init() {
            this.DoInit();
        }
    }
    
    public partial class ExecutionAbort : ModellMeister.Runtime.IModelType {
        
        private bool _Condition;
        
        partial void DoInit();
        
        partial void DoExecute(ModellMeister.Runtime.StepInfo info);
        
        [ModellMeister.Runtime.Port(ModellMeister.Runtime.PortType.Input)]
        public bool Condition {
            get {
                return this._Condition;
            }
            set {
                this._Condition = value;
            }
        }
        
        public void Execute(ModellMeister.Runtime.StepInfo info) {
            this.DoExecute(info);
        }
        
        public void Init() {
            this.DoInit();
        }
    }
}
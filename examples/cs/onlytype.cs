//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.34209
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ModelBased {
    
    
    [ModellMeister.Runtime.RootModelAttribute()]
    public partial class _ : ModellMeister.Runtime.IModelType {
        
        public void Init() {
        }
        
        public void Execute(ModellMeister.Runtime.StepInfo info) {
        }
    }
    
    public partial class Adder : ModellMeister.Runtime.IModelType {
        
        private double _Summand1;
        
        private double _Summand2;
        
        private double _Sum;
        
        partial void DoInit();
        
        partial void DoExecute(ModellMeister.Runtime.StepInfo info);
        
        [ModellMeister.Runtime.Port(ModellMeister.Runtime.PortType.Input)]
        public double Summand1 {
            get {
                return this._Summand1;
            }
            set {
                this._Summand1 = value;
            }
        }
        
        [ModellMeister.Runtime.Port(ModellMeister.Runtime.PortType.Input)]
        public double Summand2 {
            get {
                return this._Summand2;
            }
            set {
                this._Summand2 = value;
            }
        }
        
        [ModellMeister.Runtime.Port(ModellMeister.Runtime.PortType.Input)]
        public double Sum {
            get {
                return this._Sum;
            }
            set {
                this._Sum = value;
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

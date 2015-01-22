//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.34014
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ModelBased {
    
    
    [ModellMeister.Runtime.RootModelAttribute()]
    public partial class _ : ModellMeister.Runtime.IModelType {
        
        private ModellMeister.Library.Source.Constant _Constant;
        
        private ModellMeister.Library.Algebra.Addition _Adder1;
        
        private CSVWriter _Writer;
        
        public ModellMeister.Library.Source.Constant Constant {
            get {
                return this._Constant;
            }
            set {
                this._Constant = value;
            }
        }
        
        public ModellMeister.Library.Algebra.Addition Adder1 {
            get {
                return this._Adder1;
            }
            set {
                this._Adder1 = value;
            }
        }
        
        public CSVWriter Writer {
            get {
                return this._Writer;
            }
            set {
                this._Writer = value;
            }
        }
        
        public void Init() {
            this._Constant = new ModellMeister.Library.Source.Constant();
            this._Constant.Input = 1D;
            this._Constant.Init();
            this._Adder1 = new ModellMeister.Library.Algebra.Addition();
            this._Adder1.Summand2 = 0D;
            this._Adder1.Init();
            this._Writer = new CSVWriter();
            this._Writer.Init();
        }
        
        public void Execute(ModellMeister.Runtime.StepInfo info) {
            this._Constant.Execute(info);
            this.Adder1.Summand1 = this.Constant.Output;
            this._Adder1.Execute(info);
            this.Writer.Input = this.Adder1.Sum;
            this._Writer.Execute(info);
            this.Adder1.Summand2 = this.Adder1.Sum;
        }
    }
}
namespace ModellMeister.Library.Algebra {
    
}
namespace ModellMeister.Library.Sink {
    
}
namespace ModellMeister.Library.Analysis {
    
}
namespace ModellMeister.Library.Comparison {
    
}
namespace ModellMeister.Library.ControlFlow {
    
}
namespace ModellMeister.Library.Helper {
    
}
namespace ModellMeister.Library.Logic {
    
}
namespace ModellMeister.Library.Statistics {
    
}
namespace ModellMeister.Library.Source {
    
}
namespace ModellMeister.Library.Sink {
    
}
namespace ModelBased {
    
    
    public partial class CSVWriter : ModellMeister.Runtime.IModelType {
        
        private double _Input;
        
        partial void DoInit();
        
        partial void DoExecute(ModellMeister.Runtime.StepInfo info);
        
        [ModellMeister.Runtime.Port(ModellMeister.Runtime.PortType.Input)]
        public double Input {
            get {
                return this._Input;
            }
            set {
                this._Input = value;
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

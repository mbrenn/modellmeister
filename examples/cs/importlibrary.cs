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
        
        private ModellMeister.Library.Algebra.Addition _Adder1;
        
        private ModellMeister.Library.Algebra.Addition _Adder2;
        
        public ModellMeister.Library.Algebra.Addition Adder1 {
            get {
                return this._Adder1;
            }
            set {
                this._Adder1 = value;
            }
        }
        
        public ModellMeister.Library.Algebra.Addition Adder2 {
            get {
                return this._Adder2;
            }
            set {
                this._Adder2 = value;
            }
        }
        
        public void Init() {
            this._Adder1 = new ModellMeister.Library.Algebra.Addition();
            this._Adder1.Init();
            this._Adder2 = new ModellMeister.Library.Algebra.Addition();
            this._Adder2.Init();
        }
        
        public void Execute(ModellMeister.Runtime.StepInfo info) {
            this._Adder1.Execute(info);
            this.Adder2.Summand1 = this.Adder1.Sum;
            this._Adder2.Execute(info);
        }
    }
}
namespace ModellMeister.Library.Algebra {
    
}
namespace ModellMeister.Library.Statistics {
    
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
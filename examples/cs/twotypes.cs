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
    
    
    public partial class _ {
        
        public void Init() {
        }
        
        public void Execute() {
        }
    }
    
    public partial class Adder {
        
        private double _Summand1;
        
        private double _Summand2;
        
        private double _Sum;
        
        partial void DoInit();
        
        partial void DoExecute();
        
        public virtual double Summand1 {
            get {
                return this._Summand1;
            }
            set {
                this._Summand1 = value;
            }
        }
        
        public virtual double Summand2 {
            get {
                return this._Summand2;
            }
            set {
                this._Summand2 = value;
            }
        }
        
        public virtual double Sum {
            get {
                return this._Sum;
            }
            set {
                this._Sum = value;
            }
        }
        
        public void Execute() {
            this.DoExecute();
        }
        
        public void Init() {
            this.DoInit();
        }
    }
    
    public partial class Multiplier {
        
        private double _Factor1;
        
        private double _Factor2;
        
        private double _Product;
        
        partial void DoInit();
        
        partial void DoExecute();
        
        public virtual double Factor1 {
            get {
                return this._Factor1;
            }
            set {
                this._Factor1 = value;
            }
        }
        
        public virtual double Factor2 {
            get {
                return this._Factor2;
            }
            set {
                this._Factor2 = value;
            }
        }
        
        public virtual double Product {
            get {
                return this._Product;
            }
            set {
                this._Product = value;
            }
        }
        
        public void Execute() {
            this.DoExecute();
        }
        
        public void Init() {
            this.DoInit();
        }
    }
}
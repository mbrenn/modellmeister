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
        
        private Sine _Source1;
        
        private Constant _Source2;
        
        private Adder _Summer;
        
        private CSVWriter _Writer;
        
        public Sine Source1 {
            get {
                return this._Source1;
            }
            set {
                this._Source1 = value;
            }
        }
        
        public Constant Source2 {
            get {
                return this._Source2;
            }
            set {
                this._Source2 = value;
            }
        }
        
        public Adder Summer {
            get {
                return this._Summer;
            }
            set {
                this._Summer = value;
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
            this._Source1 = new Sine();
            this._Source1.Init();
            this._Source2 = new Constant();
            this._Source2.Init();
            this._Summer = new Adder();
            this._Summer.Init();
            this._Writer = new CSVWriter();
            this._Writer.Init();
        }
        
        public void Execute() {
            this._Source1.Execute();
            this._Source2.Execute();
            this.Summer.Summand1 = this.Source1.Output;
            this.Summer.Summand2 = this.Source2.Output;
            this._Summer.Execute();
            this.Writer.Input = this.Summer.Sum;
            this._Writer.Execute();
        }
    }
    
    public partial class Sine {
        
        private double _Output;
        
        partial void DoInit();
        
        partial void DoExecute();
        
        public virtual double Output {
            get {
                return this._Output;
            }
            set {
                this._Output = value;
            }
        }
        
        public void Execute() {
            this.DoExecute();
        }
        
        public void Init() {
            this.DoInit();
        }
    }
    
    public partial class Constant {
        
        private double _Output;
        
        partial void DoInit();
        
        partial void DoExecute();
        
        public virtual double Output {
            get {
                return this._Output;
            }
            set {
                this._Output = value;
            }
        }
        
        public void Execute() {
            this.DoExecute();
        }
        
        public void Init() {
            this.DoInit();
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
    
    public partial class CSVWriter {
        
        private double _Input;
        
        partial void DoInit();
        
        partial void DoExecute();
        
        public virtual double Input {
            get {
                return this._Input;
            }
            set {
                this._Input = value;
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
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
        
        public void Execute(ModellMeister.Runtime.StepInfo info) {
            this._Source1.Execute(info);
            this._Source2.Execute(info);
            this.Summer.Summand1 = this.Source1.Output;
            this.Summer.Summand2 = this.Source2.Output;
            this._Summer.Execute(info);
            this.Writer.Input = this.Summer.Sum;
            this._Writer.Execute(info);
        }
    }
    
    public partial class Sine : ModellMeister.Runtime.IModelType {
        
        private double _Output;
        
        partial void DoInit();
        
        partial void DoExecute(ModellMeister.Runtime.StepInfo info);
        
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
    
    public partial class Constant : ModellMeister.Runtime.IModelType {
        
        private double _Output;
        
        partial void DoInit();
        
        partial void DoExecute(ModellMeister.Runtime.StepInfo info);
        
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
    
    public partial class Adder : ModellMeister.Runtime.IModelType {
        
        private double _Summand1;
        
        private double _Summand2;
        
        private double _Sum;
        
        partial void DoInit();
        
        partial void DoExecute(ModellMeister.Runtime.StepInfo info);
        
        public double Summand1 {
            get {
                return this._Summand1;
            }
            set {
                this._Summand1 = value;
            }
        }
        
        public double Summand2 {
            get {
                return this._Summand2;
            }
            set {
                this._Summand2 = value;
            }
        }
        
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
    
    public partial class CSVWriter : ModellMeister.Runtime.IModelType {
        
        private double _Input;
        
        partial void DoInit();
        
        partial void DoExecute(ModellMeister.Runtime.StepInfo info);
        
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

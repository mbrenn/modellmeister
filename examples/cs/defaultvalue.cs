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
        
        private ModelBased.Constant _C1;
        
        private ModelBased.Constant _C2;
        
        private ModelBased.Adder _Add;
        
        private ModelBased.CSVWriter _Writer;
        
        public ModelBased.Constant C1 {
            get {
                return this._C1;
            }
            set {
                this._C1 = value;
            }
        }
        
        public ModelBased.Constant C2 {
            get {
                return this._C2;
            }
            set {
                this._C2 = value;
            }
        }
        
        public ModelBased.Adder Add {
            get {
                return this._Add;
            }
            set {
                this._Add = value;
            }
        }
        
        public ModelBased.CSVWriter Writer {
            get {
                return this._Writer;
            }
            set {
                this._Writer = value;
            }
        }
        
        public void Init() {
            this._C1 = new ModelBased.Constant();
            this._C1.Init();
            this._C2 = new ModelBased.Constant();
            this._C2.Value = 3D;
            this._C2.Init();
            this._Add = new ModelBased.Adder();
            this._Add.Init();
            this._Writer = new ModelBased.CSVWriter();
            this._Writer.Init();
        }
        
        public void Execute(ModellMeister.Runtime.StepInfo info) {
            this._C1.Execute(info);
            this._C2.Execute(info);
            this.Add.Summand1 = this.C1.Output;
            this.Add.Summand2 = this.C2.Output;
            this._Add.Execute(info);
            this.Writer.Input = this.Add.Sum;
            this._Writer.Execute(info);
        }
    }
    
    public partial class Sine : ModellMeister.Runtime.IModelType {
        
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
    
    public partial class Constant : ModellMeister.Runtime.IModelType {
        
        private double _Value = 1D;
        
        private double _Output;
        
        partial void DoInit();
        
        partial void DoExecute(ModellMeister.Runtime.StepInfo info);
        
        [ModellMeister.Runtime.Port(ModellMeister.Runtime.PortType.Input)]
        public double Value {
            get {
                return this._Value;
            }
            set {
                this._Value = value;
            }
        }
        
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

//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.34014
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ModelBased
{
    
    
    public partial class _
    {
        
        private Summer _FirstSummer;
        
        private Summer _SecondSummer;
        
        public Summer FirstSummer
        {
            get
            {
                return this._FirstSummer;
            }
            set
            {
                this._FirstSummer = value;
            }
        }
        
        public Summer SecondSummer
        {
            get
            {
                return this._SecondSummer;
            }
            set
            {
                this._SecondSummer = value;
            }
        }
        
        public void Init()
        {
            this._FirstSummer = new Summer();
            this._FirstSummer.Init();
            this._SecondSummer = new Summer();
            this._SecondSummer.Init();
        }
        
        public void Execute()
        {
            this._FirstSummer.Execute();
            this.SecondSummer.Summand1 = this.FirstSummer.Sum;
            this._SecondSummer.Execute();
        }
    }
    
    public partial class Summer
    {
        
        private double _Summand1;
        
        private double _Summand2;
        
        private double _Sum;
        
        public virtual double Summand1
        {
            get
            {
                return this._Summand1;
            }
            set
            {
                this._Summand1 = value;
            }
        }
        
        public virtual double Summand2
        {
            get
            {
                return this._Summand2;
            }
            set
            {
                this._Summand2 = value;
            }
        }
        
        public virtual double Sum
        {
            get
            {
                return this._Sum;
            }
            set
            {
                this._Sum = value;
            }
        }
        
        public void Execute()
        {
        }
        
        public void Init()
        {
        }
    }
}

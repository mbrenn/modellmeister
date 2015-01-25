using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbsim
{
    public class ProgramArguments
    {
        [Option('f', "file", Required = true, HelpText = "Path to dll being used as a model")]
        public string File
        {
            get;
            set;
        }

        [Option('t', 
            "time", 
            Required = true, 
            HelpText = "Path to dll being used as a model", 
            DefaultValue = 10.0)]
        public double SimulationTime
        {
            get;
            set;
        }

        [Option('i', 
            "interval", 
            Required = true, 
            HelpText = "Path to dll being used as a model", 
            DefaultValue = 0.01)]
        public double SimulationInterval
        {
            get;
            set;
        }
    }
}

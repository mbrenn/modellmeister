using BurnSystems.CommandLine.ByAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbsim
{
    public class ProgramArguments
    {
        [NamedArgument(LongName = "file", ShortName = 'f', IsRequired = true, HelpText = "Path to dll being used as a model")]
        public string File
        {
            get;
            set;
        }

        [NamedArgument(LongName = "time",
            ShortName = 't',
            IsRequired = true,
            HelpText = "Path to dll being used as a model",
            DefaultValue = "10.0")]
        public double SimulationTime
        {
            get;
            set;
        }

        [NamedArgument(LongName = "interval",
            ShortName = 'i',
            IsRequired = true,
            HelpText = "Path to dll being used as a model",
            DefaultValue = "0.01")]
        public double SimulationInterval
        {
            get;
            set;
        }
    }
}

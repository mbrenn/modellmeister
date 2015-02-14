using BurnSystems.CommandLine.ByAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbgi2cs
{
    public class ProgramArguments
    {
        [NamedArgument(LongName = "in", ShortName = 'i', IsRequired = true, HelpText = ".mbgi-File to be parsed ")]
        public string InputFile
        {
            get;
            set;
        }

        [NamedArgument(LongName = "out", ShortName = 'o', IsRequired = true, HelpText = ".cs-File being outputted")]
        public string OutputFile
        {
            get;
            set;
        }

        [NamedArgument(LongName = "dll",
            HelpText = ".dll file to be outputted, can be omitted")]
        public string DoCompileDll
        {
            get;
            set;
        }
    }
}

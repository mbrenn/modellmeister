using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbgi2cs
{
    public class ProgramArguments
    {
        [Option('i', "in", Required = true, HelpText = ".mbgi-File to be parsed ")]
        public string InputFile
        {
            get;
            set;
        }

        [Option('o', "out", Required = true)]
        public string OutputFile
        {
            get;
            set;
        }

        [Option(longName: "dll",
            DefaultValue = "",
            HelpText = "The resulting sourcefile is compiled to a dll.")]
        public string DoCompileDll
        {
            get;
            set;
        }
    }
}

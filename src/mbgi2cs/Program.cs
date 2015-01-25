using CommandLine;
using CommandLine.Text;
using ModellMeister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbgi2cs
{
    class Program
    {
        static void Main(string[] args)
        {
            var arguments = new ProgramArguments();
            var result = CommandLine.Parser.Default.ParseArguments<ProgramArguments>(args);
            if (!result.Errors.Any())
            {
                Console.WriteLine("Model Based Generation Instruction File to C#-Converter");

                var converter = new Mbgi2CsConverter();

                var sourceFile = result.Value.InputFile;
                var targetFile = result.Value.OutputFile;

                Console.WriteLine("Source File: " + result.Value.InputFile);
                Console.WriteLine("Target File: " + result.Value.OutputFile);

                Console.WriteLine();
                Console.WriteLine("Start of the conversion");

                converter.ConvertFile(sourceFile, targetFile);
            }
        }
    }
}

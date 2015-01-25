using CommandLine;
using CommandLine.Text;
using ModellMeister;
using System;
using System.Collections.Generic;
using System.IO;
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

                Console.WriteLine("Source File: " + sourceFile);
                Console.WriteLine("Target File: " + targetFile);

                Console.WriteLine();
                Console.WriteLine("- Start of the conversion");

                converter.ConvertFile(sourceFile, targetFile);

                // Checks, if compilation is requested
                if (!string.IsNullOrEmpty(result.Value.DoCompileDll))
                {
                    Console.WriteLine("- Start of the compilation");
                    var compiler = new Mb2DllCompiler();
                    var importedAssemblies = converter.ImportedAssemblies;
                    compiler.AddLibraries(importedAssemblies);
                    compiler.CompileSourceCode(
                        Path.GetDirectoryName(targetFile),
                        new string[] { targetFile },
                        result.Value.DoCompileDll).Wait();
                }
            }
        }
    }
}

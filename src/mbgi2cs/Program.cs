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
            Console.WriteLine("Model Based Generation Instruction File to C#-Converter");

            var converter = new Mbgi2CsConverter();

            if (args.Length != 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("mbgi2cs.exe sourcefile.mbgi targetfile.cs");
                Console.WriteLine();
                Console.WriteLine("Converts the sourcefile.mbgi to a C# file in targetfile.cs");
                return;
            }

            var sourceFile = args[0];
            var targetFile = args[1];

            Console.WriteLine("Source File: " + args[0]);
            Console.WriteLine("Target File: " + args[1]);

            Console.WriteLine();

            Console.WriteLine("Start of the conversion");

            converter.ConvertFile(sourceFile, targetFile);
        }
    }
}

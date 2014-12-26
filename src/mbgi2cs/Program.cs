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
        }
    }
}

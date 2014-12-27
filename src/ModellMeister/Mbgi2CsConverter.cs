using ModellMeister.FileParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister
{
    /// <summary>
    /// This file performs the actual conversion
    /// </summary>
    public class Mbgi2CsConverter
    {
        public void ConvertFile(string pathSourceFile, string pathTargetFile)
        {
            if (!File.Exists(pathSourceFile))
            {
                throw new InvalidOperationException("Source file was not found: " + pathSourceFile);
            }

            var reader = new StreamReader(pathSourceFile);
            var writer = new StreamWriter(pathTargetFile);

            this.ConvertFile(reader, writer);
        }

        public void ConvertFile(StreamReader reader, StreamWriter writer)
        {
            var parser = new MbgiFileParser();
            parser.ParseFile(reader);
        }
    }
}

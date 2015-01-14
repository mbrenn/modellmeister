﻿using ModellMeister.FileParser;
using ModellMeister.SourceGenerator.CSharp;
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

            using (var reader = new StreamReader(pathSourceFile))
            {
                using (var writer = new StreamWriter(pathTargetFile))
                {
                    this.ConvertStreams(
                        Path.GetDirectoryName(pathSourceFile),
                        reader,
                        writer);
                }
            }
        }

        /// <summary>
        /// Converts the file by text reader and writer
        /// </summary>
        /// <param name="reader">Reader which will contain the MBGI Code</param>
        /// <param name="writer">Writer which will contain the C# code</param>
        public void ConvertStreams(string pathOfContext, TextReader reader, TextWriter writer)
        {
            Console.WriteLine("Parsing MBGI File");
            var parser = new MbgiFileParser();
            var model = parser.ParseFileFromReader(pathOfContext, reader);

            Console.WriteLine("Writing C#-Code");
            var generator = new CSharpGenerator();
            generator.CreateSource(model, writer);
        }
    }
}

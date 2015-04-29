using BurnSystems.Logger;
using ModellMeister.FileParser;
using ModellMeister.SourceGenerator.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Compiler
{
    /// <summary>
    /// This file performs the actual conversion
    /// </summary>
    public class Mbgi2CsConverter
    {
        private static ClassLogger logger = new ClassLogger(typeof(Mbgi2CsConverter));

        /// <summary>
        /// Stores the list of imported assemblies
        /// </summary>
        private List<string> importedAssemblies;

        /// <summary>
        /// Stores the list of imported assemblies
        /// </summary>
        private List<string> importedFiles;

        /// <summary>
        /// Gets the list of imported assemblies
        /// </summary>
        public List<string> ImportedAssemblies
        {
            get { return this.importedAssemblies; }
        }

        /// <summary>
        /// Gets the list of imported assemblies
        /// </summary>
        public List<string> ImportedFiles
        {
            get { return this.importedFiles; }
        }

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
            logger.Notify("Parsing MBGI File");
            var parser = new MbgiFileParser();
            var model = parser.ParseFileFromReader(pathOfContext, reader);

            logger.Notify("Writing C#-Code");
            var generator = new CSharpGenerator();
            generator.CreateSource(model, writer);

            this.importedAssemblies = parser.ImportedAssemblies;
            this.importedFiles = parser.ImportedFiles;
        }
    }
}

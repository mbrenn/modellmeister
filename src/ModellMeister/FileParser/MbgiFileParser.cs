using ModellMeister.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.FileParser
{
    public class MbgiFileParser
    {
        /// <summary>
        /// Defines the current parsing scope
        /// </summary>
        private CurrentScope currentScope = CurrentScope.Global;

        /// <summary>
        /// Stores the current tpye
        /// </summary>
        private ModelType currentType;

        public CompositeType ParseFileFromText(string loadedFile)
        {
            var reader = new StringReader(loadedFile);
            return this.ParseFile(reader);
        }

        /// <summary>
        /// Parses the file and includes the information. This method is not reentrable
        /// </summary>
        /// <param name="reader">The reader to be used</param>
        /// <returns>The created environment containing the complete information</returns>
        public CompositeType ParseFile(TextReader reader)
        {
            var result = new CompositeType();
            result.Name = "_";

            this.currentScope = CurrentScope.Global;

            var lineParser = new LineParser();
            foreach (var line in lineParser.ParseFile(reader))
            {
                if (line.LineType == Model.EntityType.Type)
                {
                    this.currentScope = CurrentScope.InType;
                    this.currentType = new ModelType();
                    this.currentType.Name = line.Name;

                    result.Types.Add(this.currentType);

                    Console.WriteLine("Type is created: " + this.currentType.Name);
                }

                if (line.LineType == EntityType.TypeInput)
                {
                    if (this.currentScope != CurrentScope.InType)
                    {
                        throw new InvalidOperationException("Unexpected scope: Expected InType");
                    }

                    var port = CreatePort(line);
                    this.currentType.Inputs.Add(port);

                    Console.WriteLine("- Input is created: " + port.Name);
                }

                if (line.LineType == EntityType.TypeOutput)
                {
                    if (this.currentScope != CurrentScope.InType)
                    {
                        throw new InvalidOperationException("Unexpected scope: Expected InType");
                    }

                    var port = CreatePort(line);
                    this.currentType.Outputs.Add(port);

                    Console.WriteLine("- Output is created: " + port.Name);
                }
            }

            return result;
        }

        /// <summary>
        /// Creates the port by a parsed line
        /// </summary>
        /// <param name="line">ine to be parsed</param>
        /// <returns>The created port</returns>
        public static Port CreatePort(ParsedLine line)
        {
            var port = new Port();
            port.Name = line.Name;
            port.DataType = LineParser.ConvertToDataType(line.GetProperty(PropertyType.OfType));
            return port;
        }

        private enum CurrentScope
        {
            Global,
            InType,
            InBlock,
            InCompositeBlock
        }
    }
}

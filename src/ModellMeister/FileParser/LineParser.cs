using ModellMeister.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.FileParser
{
    /// <summary>
    /// Defines the line parser, which parses the file into lines and prepare the 
    /// parsed lines
    /// </summary>
    public class LineParser
    {
        /// <summary>
        /// Stores the stream reader being used
        /// </summary>
        private StreamReader reader;

        /// <summary>
        /// Stores the model types
        /// </summary>
        private static Dictionary<string, ModelType> modelTypes = new Dictionary<string, ModelType>();

        /// <summary>
        /// Stores the model types
        /// </summary>
        private static Dictionary<string, PropertyType> propertyTypes = new Dictionary<string, PropertyType>();

        static LineParser()
        {
            modelTypes["B"] = ModelType.Block;
            modelTypes["BI"] = ModelType.BlockInput;
            modelTypes["BO"] = ModelType.BlockOutput;
            modelTypes["C"] = ModelType.CompositeType;
            modelTypes["CI"] = ModelType.CompositeTypeInput;
            modelTypes["CO"] = ModelType.CompositeTypeOutput;
            modelTypes["S"] = ModelType.Setting;
            modelTypes["T"] = ModelType.Type;
            modelTypes["TI"] = ModelType.TypeInput;
            modelTypes["TO"] = ModelType.TypeOutput;
            modelTypes["W"] = ModelType.Wire;

            propertyTypes[":"] = PropertyType.OfType;
            propertyTypes["defaultvalue"] = PropertyType.DefaultValue;
        }


        /// <summary>
        /// Initializes a new instance of the LineParser class
        /// </summary>
        /// <param name="reader"></param>
        public void ParseFile(StreamReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            this.reader = reader;
        }

        public IEnumerable<ParsedLine> GetLines()
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var parsedLine = this.ParseLine(line);
                if (parsedLine != null)
                {
                    yield return parsedLine;
                }
            }
        }

        /// <summary>
        /// Parses the line
        /// </summary>
        /// <param name="line">Line to be parsed</param>
        /// <returns>Returns the parsed line</returns>
        public ParsedLine ParseLine(string line)
        {
            // Remove comment
            var indexHash = line.IndexOf('#');
            if (indexHash >= 0)
            {
                line = line.Substring(0, indexHash);
            }

            line = line.Trim();

            if (String.IsNullOrWhiteSpace(line))
            {
                // Nothing to return, there is no statement in
                return null;
            }

            // Split everything, seems to be a model line
            var parts = line.Split(new char[] { ' ', '\t' })
                .Where (x=> !string.IsNullOrWhiteSpace(x))
                .ToList();

            var parsedLine = new ParsedLine();

            // Sets the model type
            ModelType found;
            if (!modelTypes.TryGetValue(parts[0].ToUpper(), out found))
            {
                throw new InvalidOperationException("The type '" + parts[0] + "' is not know");
            }

            parsedLine.LineType = found;

            // Sets the name
            if (parts.Count > 1)
            {
                parsedLine.Name = parts[1];
            }

            // Sets the other variables
            var n = 2;
            while (n < parts.Count)
            {
                var key = parts[n];
                var value = parts[n + 1];

                PropertyType foundPropertyType;
                if (!propertyTypes.TryGetValue(key, out foundPropertyType))
                {
                    throw new InvalidOperationException("Unknown property type '" + key + "'");
                }

                parsedLine.Parameters[foundPropertyType] = value;

                n += 2;
            }
            
            return parsedLine;
        }
    }
}

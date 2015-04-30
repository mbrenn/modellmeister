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
        /// Stores the model types
        /// </summary>
        private static Dictionary<string, EntityType> modelTypes = new Dictionary<string, EntityType>();

        /// <summary>
        /// Stores the property types
        /// </summary>
        private static Dictionary<string, PropertyType> propertyTypes = new Dictionary<string, PropertyType>();

        /// <summary>
        /// Stores the data types
        /// </summary>
        private static Dictionary<string, DataType> dataTypes = new Dictionary<string, DataType>();

        public static Dictionary<string, EntityType> ModelTypes
        {
            get { return modelTypes; }
        }

        public static Dictionary<string, PropertyType> PropertyTypes
        {
            get { return propertyTypes; }
        }

        public static Dictionary<string, DataType> DataTypes
        {
            get { return dataTypes; }
        }

        static LineParser()
        {
            modelTypes["B"] = EntityType.Block;
            modelTypes["BI"] = EntityType.BlockInput;
            modelTypes["BO"] = EntityType.BlockOutput;
            modelTypes["C"] = EntityType.CompositeType;
            modelTypes["CI"] = EntityType.CompositeTypeInput;
            modelTypes["CO"] = EntityType.CompositeTypeOutput;
            modelTypes["CB"] = EntityType.CompositeBlock;
            modelTypes["CBI"] = EntityType.CompositeBlockInput;
            modelTypes["CW"] = EntityType.CompositeWire;
            modelTypes["CF"] = EntityType.CompositeFeedback;
            modelTypes["S"] = EntityType.Setting;
            modelTypes["N"] = EntityType.NameSpace;
            modelTypes["T"] = EntityType.Type;
            modelTypes["TI"] = EntityType.TypeInput;
            modelTypes["TO"] = EntityType.TypeOutput;
            modelTypes["W"] = EntityType.Wire;
            modelTypes["F"] = EntityType.Feedback;
            modelTypes["!I"] = EntityType.CommandImportFile;
            modelTypes["!L"] = EntityType.CommandLoadLibrary;

            propertyTypes[":"] = PropertyType.OfType;
            propertyTypes["defaultvalue"] = PropertyType.DefaultValue;

            dataTypes["Double"] = DataType.Double;
            dataTypes["Integer"] = DataType.Integer;
            dataTypes["String"] = DataType.String;
            dataTypes["Boolean"] = DataType.Boolean;
        }


        /// <summary>
        /// Initializes a new instance of the LineParser class
        /// </summary>
        /// <param name="reader"></param>
        public IEnumerable<ParsedLine> ParseFile(TextReader reader, string filename)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            return this.GetLines(reader, filename);
        }

        private IEnumerable<ParsedLine> GetLines(TextReader reader, string filename)
        {
            string line;
            var n = 0; 
            while ((line = reader.ReadLine()) != null)
            {
                var parsedLine = this.ParseLine(line, n, filename);
                if (parsedLine != null)
                {
                    yield return parsedLine;
                }

                n++;
            }
        }

        /// <summary>
        /// Parses the line
        /// </summary>
        /// <param name="line">Line to be parsed</param>
        /// <returns>Returns the parsed line</returns>
        public ParsedLine ParseLine(string line, int lineNumber = 0, string filename = "<< inline >>")
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
            parsedLine.RawLineContent = line;
            parsedLine.LineNumber = lineNumber;
            parsedLine.Filename = filename;

            // Sets the model type
            EntityType found;
            if (!modelTypes.TryGetValue(parts[0].ToUpper(), out found))
            {
                parsedLine.ThrowExceptionOnLine("The type '" + parts[0] + "' is not know");
            }

            parsedLine.LineType = found;

            // Sets the name
            if (parts.Count > 1)
            {
                parsedLine.Name = parts[1];
            }

            // Sets the other variables
            if (parsedLine.LineType == EntityType.Wire
                || parsedLine.LineType == EntityType.Feedback
                || parsedLine.LineType == EntityType.CompositeWire
                || parsedLine.LineType == EntityType.CompositeFeedback
                || parsedLine.LineType == EntityType.CommandImportFile
                || parsedLine.LineType == EntityType.CommandLoadLibrary)
            {
                for (var n = 1; n < parts.Count; n++)
                {
                    parsedLine.Arguments.Add(parts[n]);
                }
            }
            else
            {
                for (var n = 2; n < parts.Count; n += 2)
                {
                    var key = parts[n];
                    if ((n + 1) == parts.Count)
                    {
                        parsedLine.ThrowExceptionOnLine("Parameter '" + key + "' has just a key and does not have a value");
                    }

                    var value = parts[n + 1];

                    var foundPropertyType = ConvertToPropertyType(parsedLine, key);

                    parsedLine.Parameters[foundPropertyType] = value;
                }
            }

            return parsedLine;
        }

        /// <summary>
        /// Converts the text to a propertytype
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static PropertyType ConvertToPropertyType(ParsedLine parsedLine, string key)
        {
            PropertyType foundPropertyType;
            if (!propertyTypes.TryGetValue(key, out foundPropertyType))
            {
                parsedLine.ThrowExceptionOnLine("Unknown property type '" + key + "'");
            }

            return foundPropertyType;
        }

        /// <summary>
        /// Converts the string to a datatype
        /// </summary>
        /// <param name="text">Text to be converted</param>
        /// <returns>The found datattype</returns>
        public static DataType ConvertToDataType(ParsedLine parsedLine, string text)
        {
            DataType result;
            if (!dataTypes.TryGetValue(text, out result))
            {
                parsedLine.ThrowExceptionOnLine("Unknown data type '" + text + "'");
            }

            return result;
        }
    }
}

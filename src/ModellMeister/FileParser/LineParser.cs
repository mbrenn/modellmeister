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
            modelTypes["CW"] = EntityType.CompositeWire;
            modelTypes["S"] = EntityType.Setting;
            modelTypes["T"] = EntityType.Type;
            modelTypes["TI"] = EntityType.TypeInput;
            modelTypes["TO"] = EntityType.TypeOutput;
            modelTypes["W"] = EntityType.Wire;

            propertyTypes[":"] = PropertyType.OfType;
            propertyTypes["defaultvalue"] = PropertyType.DefaultValue;

            dataTypes["Double"] = DataType.Double;
            dataTypes["Integer"] = DataType.Integer;
            dataTypes["String"] = DataType.String;
        }


        /// <summary>
        /// Initializes a new instance of the LineParser class
        /// </summary>
        /// <param name="reader"></param>
        public IEnumerable<ParsedLine> ParseFile(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            return this.GetLines(reader);
        }

        private IEnumerable<ParsedLine> GetLines(TextReader reader)
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
            EntityType found;
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
            if (parsedLine.LineType == EntityType.Wire
                || parsedLine.LineType == EntityType.CompositeWire)
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
                        throw new InvalidOperationException("Parameter '" + key + "' has just a key and does not have a value");
                    }

                    var value = parts[n + 1];

                    PropertyType foundPropertyType = ConvertToPropertyType(key);

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
        public static PropertyType ConvertToPropertyType(string key)
        {
            PropertyType foundPropertyType;
            if (!propertyTypes.TryGetValue(key, out foundPropertyType))
            {
                throw new InvalidOperationException("Unknown property type '" + key + "'");
            }

            return foundPropertyType;
        }

        /// <summary>
        /// Converts the string to a datatype
        /// </summary>
        /// <param name="text">Text to be converted</param>
        /// <returns>The found datattype</returns>
        public static DataType ConvertToDataType(string text)
        {
            DataType result;
            if (!dataTypes.TryGetValue(text, out result))
            {
                throw new InvalidOperationException("Unknown data type '" + text + "'");
            }

            return result;
        }
    }
}

using ModellMeister.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Logic
{
    public static class Conversion
    {
        /// <summary>
        /// Converts a given text value to a datatype instance
        /// </summary>
        /// <param name="textValue">Value of the text to be converted</param>
        /// <param name="dataType">DataType to which the value will be converted</param>
        /// <returns>The converted object</returns>
        public static object ToDataType(string textValue, DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Double:
                    return Convert.ToDouble(textValue, CultureInfo.InvariantCulture);
                case DataType.Integer:
                    return Convert.ToInt32(textValue, CultureInfo.InvariantCulture);
                case DataType.String:
                    return textValue;
                default:
                    throw new InvalidOperationException("Unknown DataType: " + dataType.ToString());
            }
        }

        public static string ConvertToDotNetType(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Double:
                    return "System.Double";
                case DataType.Integer:
                    return "System.Int32";
                case DataType.String:
                    return "System.String";
                case DataType.Boolean:
                    return "System.Boolean";
                default:
                    throw new InvalidOperationException("Unknown Type: " + dataType.ToString());
            }
        }

        public static DataType ConvertToDataType(Type dataType)
        {
            if (dataType == typeof(System.Double))
            {
                return DataType.Double;
            }

            if (dataType == typeof(System.String))
            {
                return DataType.String;
            }

            if (dataType == typeof(System.Int32))
            {
                return DataType.Integer;
            }

            if (dataType == typeof(System.Boolean))
            {
                return DataType.Boolean;
            }

            throw new InvalidOperationException("Unknown Type: " + dataType.ToString());
        }

        public static object ConvertToDotNetValue(DataType dataType, object value)
        {
            switch (dataType)
            {
                case DataType.Double:
                    return Convert.ToDouble(value, CultureInfo.InvariantCulture);
                case DataType.Integer:
                    return Convert.ToInt32(value, CultureInfo.InvariantCulture);
                case DataType.String:
                    return Convert.ToString(value, CultureInfo.InvariantCulture);
                default:
                    throw new InvalidOperationException("Unknown Type: " + dataType.ToString());
            }
        }
    }
}

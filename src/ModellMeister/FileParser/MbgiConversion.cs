using ModellMeister.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.FileParser
{
    /// <summary>
    /// Converter for String to Datatype
    /// </summary>
    public static class MbgiConversion
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
    }
}

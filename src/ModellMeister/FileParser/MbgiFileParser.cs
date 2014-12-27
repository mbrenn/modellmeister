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
        /// Parses the file and includes the information
        /// </summary>
        /// <param name="path">Path to be</param>
        public void ParseFile(StreamReader reader)
        {
            var lineParser = new LineParser();
            foreach (var line in lineParser.ParseFile(reader))
            {
            }
        }
    }
}

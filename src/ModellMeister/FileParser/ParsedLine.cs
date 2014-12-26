using ModellMeister.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.FileParser
{
    /// <summary>
    /// Stores the information of a parsed line
    /// </summary>
    public class ParsedLine
    {
        public ModelType LineType
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public Dictionary<PropertyType, string> Parameters
        {
            get;
            private set;
        }

        public ParsedLine()
        {
            this.Parameters = new Dictionary<PropertyType, string>();
        }

        public ParsedLine(ModelType type, string name)
            : this()
        {
            this.LineType = type;
            this.Name = name;
        }
    }
}

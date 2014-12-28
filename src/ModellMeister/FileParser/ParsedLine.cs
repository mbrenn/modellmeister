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
        public EntityType LineType
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public List<string> Arguments
        {
            get;
            set;
        }

        public Dictionary<PropertyType, string> Parameters
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the ParsedLine class.
        /// </summary>
        public ParsedLine()
        {
            this.Parameters = new Dictionary<PropertyType, string>();
            this.Arguments = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the ParsedLine class.
        /// </summary>
        /// <param name="type">Type to be queried</param>
        /// <param name="name">Name of the object</param>
        /// 
        public ParsedLine(EntityType type, string name)
            : this()
        {
            this.LineType = type;
            this.Name = name;
        }

        /// <summary>
        /// Gets the property or returns null, if the property does not exist
        /// </summary>
        /// <param name="type">Type to be queried</param>
        /// <returns>the content or null if not found</returns>
        public string GetProperty(PropertyType type)
        {
            string result;
            if (this.Parameters.TryGetValue(type, out result))
            {
                return result;
            }

            return null;
        }
    }
}

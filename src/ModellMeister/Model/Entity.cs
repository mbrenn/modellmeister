using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Model
{
    /// <summary>
    /// Describes the general entity
    /// </summary>
    public abstract class Entity
    {
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Converts the entity to a string
        /// </summary>
        /// <returns>The converted entity</returns>
        public override string ToString()
        {
            if (this.Name != null)
            {
                return "[" + this.GetType().Name + "] " + this.Name.ToString();
            }
            else
            {
                return "[" + this.GetType().Name + "] null";
            }
        }
    }
}

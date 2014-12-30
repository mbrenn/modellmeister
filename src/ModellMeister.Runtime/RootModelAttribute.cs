using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModellMeister.Runtime
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class RootModelAttribute : Attribute
    {
        // This is a positional argument
        public RootModelAttribute()
        {            
        }
    }
}

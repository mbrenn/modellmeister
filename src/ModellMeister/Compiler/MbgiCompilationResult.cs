using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Compiler
{
    public class MbgiCompilationResult
    {
        /// <summary>
        /// Gets or sets the information about the compiled assembly
        /// </summary>
        public CompilerResults CompiledAssembly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the path to the assembly
        /// </summary>
        public string PathToAssembly
        {
            get;
            set;
        }
    }
}

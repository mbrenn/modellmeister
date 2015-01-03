﻿using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister
{
    /// <summary>
    /// Compiles the C# files for the model based and returns a .dll
    /// </summary>
    public class Mb2DllCompiler
    {
        /// <summary>
        /// Compiles the source code
        /// </summary>
        /// <param name="workspacePath">The path, where the binaries will be located</param>
        /// <param name="pathCsFiles">The list of paths for C# files to be compiled</param>
        /// <param name="pathDll">The path, where final .dll will be located</param>
        /// <returns>The compiler results</returns>
        public async Task<CompilerResults> CompileSourceCode(string workspacePath, List<string> pathCsFiles, string pathDll)
        {
            // Start the compilation
            var compiler = new CSharpCodeProvider();
            var parameters = new CompilerParameters();
            parameters.GenerateInMemory = false;
            parameters.OutputAssembly = pathDll;
            parameters.IncludeDebugInformation = true;
            parameters.CompilerOptions = "/nologo /target:library";
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Runtime.dll");
            parameters.ReferencedAssemblies.Add("System.Diagnostics.Debug.dll");
            parameters.ReferencedAssemblies.Add("ModellMeister.Runtime.dll");

            // Copies the ModellMeister.Runtime.dll to path
            File.Copy(
                Path.Combine(
                    Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                    "ModellMeister.Runtime.dll"),
                Path.Combine(workspacePath, "ModellMeister.Runtime.dll"),
                true);
            File.Copy(
                Path.Combine(
                    Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                    "ModellMeister.dll"),
                Path.Combine(workspacePath, "ModellMeister.dll"),
                true);

            return await Task.Run(() =>
                {
                    var compileResult = compiler.CompileAssemblyFromFile(parameters, pathCsFiles.ToArray());

                    if (compileResult.Errors.Cast<CompilerError>().Any(x => x.ErrorNumber == "CS0042"))
                    {
                        // When the .pdb files are loaded by Visual Studio, we cannot generate Debuginfo
                        // http://msdn.microsoft.com/de-de/library/82h240ac(v=vs.90).aspx
                        parameters.IncludeDebugInformation = false;
                        compileResult = compiler.CompileAssemblyFromFile(parameters, pathCsFiles.ToArray());
                    }

                    return compileResult;
                });
        }
    }
}
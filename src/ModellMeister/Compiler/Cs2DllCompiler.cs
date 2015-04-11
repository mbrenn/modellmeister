using BurnSystems.Logger;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Compiler
{
    /// <summary>
    /// Compiles the C# files for the model based and returns a .dll
    /// </summary>
    public class Cs2DllCompiler
    {
        /// <summary>
        /// Defines the logger to be used
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(Cs2DllCompiler));

        /// <summary>
        /// Stores the list of assemblies
        /// </summary>
        private List<string> importedAssemblies = new List<string>();

        /// <summary>
        /// Compiles the source code
        /// </summary>
        /// <param name="workspacePath">The path, where the binaries will be located</param>
        /// <param name="pathCsFiles">The list of paths for C# files to be compiled</param>
        /// <param name="pathDll">The path, where final .dll will be located</param>
        /// <returns>The compiler results</returns>
        public async Task<CompilerResults> CompileSourceCode(
            string workspacePath, 
            IEnumerable<string> pathCsFiles, 
            string pathDll)
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

            foreach (var assembly in this.importedAssemblies)
            {
                parameters.ReferencedAssemblies.Add(Path.GetFileName(assembly));
            }

            CopyAssemblies(workspacePath);

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

        /// <summary>
        /// Adds the libraries 
        /// </summary>
        /// <param name="importedAssemblies"></param>
        public void AddLibraries(List<string> importedAssemblies)
        {
            foreach (var item in importedAssemblies)
            {
                this.importedAssemblies.Add(item);
            }
        }

        /// <summary>
        /// Copies the assemblies to the workspace path
        /// </summary>
        /// <param name="workspacePath">Path, where workspace is located</param>
        public static void CopyAssemblies(string workspacePath)
        {
            // Copies the ModellMeister.Runtime.dll to path
            CopyFileIntoWorkspace(workspacePath, "ModellMeister.dll");
            CopyFileIntoWorkspace(workspacePath, "ModellMeister.Runtime.dll");
            CopyFileIntoWorkspace(workspacePath, "ModellMeister.pdb");
            CopyFileIntoWorkspace(workspacePath, "ModellMeister.Runtime.pdb");
        }

        /// <summary>
        /// Copies the file into the workspace
        /// </summary>
        /// <param name="workspacePath">Path of the workspace</param>
        /// <param name="fileName">Filename to be copied</param>
        public static void CopyFileIntoWorkspace(string workspacePath, string fileName)
        {
            try
            {
                File.Copy(
                    Path.Combine(
                        Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                        fileName),
                    Path.Combine(workspacePath, fileName),
                    true);
            }
            catch (Exception exc)
            {
                logger.Fail("Could not copy file: " + fileName + ": " + exc.Message);
            }
        }
    }
}

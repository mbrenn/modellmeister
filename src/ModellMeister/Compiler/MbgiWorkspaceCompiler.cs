using ModellMeister.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Compiler
{
    public class MbgiWorkspaceCompiler
    {
        /// <summary>
        /// Defines the log sink to be used
        /// </summary>
        private ILogSink logSink;

        /// <summary>
        /// Initializes a new SimulationWorkspace instance
        /// </summary>
        /// <param name="logSink">Log sink being used</param>
        public MbgiWorkspaceCompiler(ILogSink logSink)
        {
            this.logSink = logSink;
        }

        /// <summary>
        /// Creates the workspace for the user by copying the necessary files
        /// </summary>
        /// <param name="binPath">The binary path, where all the compilation results shall be put to</param>
        public void PrepareWorkspace(string binPath)
        {
            if (!Directory.Exists(binPath))
            {
                Directory.CreateDirectory(binPath);
            }

            // Copies the library
            Cs2DllCompiler.CopyFileIntoWorkspace
                (binPath, "ModellMeister.Library.dll");
            Cs2DllCompiler.CopyFileIntoWorkspace
                (binPath, "ModellMeister.Library.pdb");
            Cs2DllCompiler.CopyAssemblies(binPath);
        }

        public async Task<MbgiCompilationResult> CompileOnMbgiFile(string filePath)
        {
            var workspacePath = Path.GetDirectoryName(filePath);
            var filename = Path.GetFileName(filePath);

            try
            {
                var binPath = Path.Combine(workspacePath, "bin");
                if (!Directory.Exists(binPath))
                {
                    Directory.CreateDirectory(binPath);
                }

                this.PrepareWorkspace(binPath);

                List<string> importedAssemblies;

                // Gets the source code
                var completePath = Path.Combine(workspacePath, filename);

                var csList = new List<string>();
                var csPath = Path.Combine(binPath, Path.ChangeExtension(filename, ".cs"));
                var resultPath = Path.Combine(binPath, filename + ".result.txt");
                csList.Add(csPath);

                using (var sourceReader = new StreamReader(completePath))
                {
                    using (var sourcewriter = new StreamWriter(csPath))
                    {
                        var converter = new Mbgi2CsConverter();
                        converter.ConvertStreams(workspacePath, sourceReader, sourcewriter);

                        importedAssemblies = converter.ImportedAssemblies;
                    }
                }

                // Writes the file for compilation
                var csMbgiPath = Path.Combine(workspacePath, Path.ChangeExtension(filename, ".mbgi"));
                var dllPath = Path.Combine(binPath, Path.ChangeExtension(filename, ".dll"));
                var csUserPath = Path.Combine(workspacePath, Path.ChangeExtension(filename, ".user.cs"));

                this.logSink.AddMessageToLog("C#-file Generated: " + csPath);
                this.logSink.AddMessageToLog("MBGI-file Generated: " + csMbgiPath);

                if (File.Exists(csUserPath))
                {
                    csList.Add(csUserPath);
                    this.logSink.AddMessageToLog("User-defined file found for: " + csUserPath);
                }
                else
                {
                    this.logSink.AddMessageToLog("No user-defined file found for: " + csUserPath);
                }

                var dllCompiler = new Cs2DllCompiler();
                dllCompiler.AddLibraries(importedAssemblies);
                var compileSource = await dllCompiler.CompileSourceCode(workspacePath, csList, dllPath);

                if (compileSource.Errors.Count > 0)
                {
                    foreach (var error in compileSource.Errors)
                    {
                        this.logSink.AddMessageToLog("Compile Error: " + error.ToString());
                    }

                    return null;
                }

                return new MbgiCompilationResult()
                {
                    CompiledAssembly = compileSource,
                    PathToAssembly = dllPath
                };
            }
            catch (Exception exc)
            {
                this.logSink.AddMessageToLog(exc.ToString());
                return null;
            }
        }
    }
}
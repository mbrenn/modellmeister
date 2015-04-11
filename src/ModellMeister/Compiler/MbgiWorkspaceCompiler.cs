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
        /// <param name="workspacePath"></param>
        public void PrepareWorkspace(string workspacePath)
        {
            var assemblyPath = Assembly.GetEntryAssembly().Location;
            Environment.CurrentDirectory = workspacePath;

            if (!Directory.Exists(workspacePath))
            {
                Directory.CreateDirectory(workspacePath);
            }

            // Copies the library
            Cs2DllCompiler.CopyFileIntoWorkspace
                (workspacePath, Path.Combine(assemblyPath, "ModellMeister.Library.dll"));
            Cs2DllCompiler.CopyFileIntoWorkspace
                (workspacePath, Path.Combine(assemblyPath, "ModellMeister.Library.pdb"));
            Cs2DllCompiler.CopyAssemblies(workspacePath);
        }

        public async Task<MbgiCompilationResult> CompileOnMbgiFile(string filePath)
        {
            var workspacePath = Path.GetDirectoryName(filePath);
            var filename = Path.GetFileName(filePath);

            try
            {
                this.PrepareWorkspace(workspacePath);
                StringBuilder generatedSource;

                List<string> importedAssemblies;

                // Gets the source code
                var completePath = Path.Combine(workspacePath, filename);
                using (var sourceReader = new StreamReader(completePath))
                {
                    using (var sourcewriter = new StringWriter())
                    {
                        var converter = new Mbgi2CsConverter();
                        converter.ConvertStreams(workspacePath, sourceReader, sourcewriter);

                        generatedSource = sourcewriter.GetStringBuilder();
                        importedAssemblies = converter.ImportedAssemblies;
                    }
                }

                var csList = new List<string>();
                var csPath = Path.Combine(workspacePath, Path.ChangeExtension(filename, ".cs"));
                File.WriteAllText(csPath, generatedSource.ToString());
                var csMbgiPath = Path.Combine(workspacePath, filename + ".mbgi");
                var dllPath = Path.Combine(workspacePath, filename + ".dll");
                var csUserPath = Path.Combine(workspacePath, filename + ".user.cs");
                var resultPath = Path.Combine(workspacePath, filename + ".result.txt");
                csList.Add(csPath);

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
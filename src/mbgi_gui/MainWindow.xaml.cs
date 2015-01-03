using mbgi_gui.Logic;
using Microsoft.CSharp;
using ModellMeister;
using ModellMeister.Runner;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace mbgi_gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string currentFilename = "modelbased";

        public MainWindow()
        {
            InitializeComponent();

            this.AddLog("Model Based Source Generator and Executor is started");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtWorkspacePath.Text = WorkspaceLogic.WorkspacePath;

            var workspacePath = this.txtWorkspacePath.Text;
            var csMbgiPath = Path.Combine(workspacePath, currentFilename + ".mbgi");
            if (File.Exists(csMbgiPath))
            {
                this.txtMBGISource.Text = File.ReadAllText(csMbgiPath);
            }

            var csUserPath = Path.Combine(workspacePath, currentFilename + ".user.cs");
            if (File.Exists(csUserPath))
            {
                this.txtUserCs.Text = File.ReadAllText(csUserPath);
            }
        }

        public void AddLog(string line)
        {
            this.txtLog.Text = "[" + DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss") + "]: " + line + "\r\n" + this.txtLog.Text;
        }

        private void btnGenerateSource_Click(object sender, RoutedEventArgs e)
        {
            var logMessage = new StringBuilder();

            try
            {
                var workspacePath = this.txtWorkspacePath.Text;
                Environment.CurrentDirectory = workspacePath;
                if (!Directory.Exists(workspacePath))
                {
                    Directory.CreateDirectory(workspacePath);
                }

                StringBuilder builder;
                // Gets the source code
                using (var sourceReader = new StringReader(this.txtMBGISource.Text))
                {
                    using (var sourcewriter = new StringWriter())
                    {
                        var converter = new Mbgi2CsConverter();
                        converter.ConvertStreams(sourceReader, sourcewriter);

                        builder = sourcewriter.GetStringBuilder();
                    }
                }

                var csList = new List<string>();
                var csPath = Path.Combine(workspacePath, currentFilename + ".cs");
                var csMbgiPath = Path.Combine(workspacePath, currentFilename + ".mbgi");
                var dllPath = Path.Combine(workspacePath, currentFilename + ".dll");
                var csUserPath = Path.Combine(workspacePath, currentFilename + ".user.cs");
                var resultPath = Path.Combine(workspacePath, currentFilename + ".result.txt");
                csList.Add(csPath);

                File.WriteAllText(csMbgiPath, this.txtMBGISource.Text);
                File.WriteAllText(csUserPath, this.txtUserCs.Text);
                File.WriteAllText(csPath, builder.ToString());

                logMessage.AppendLine("C#-file Generated: " + csPath);
                logMessage.AppendLine("MBGI-file Generated: " + csMbgiPath);

                if (File.Exists(csUserPath))
                {
                    csList.Add(csUserPath);
                    logMessage.AppendLine("User-defined file found for: " + csUserPath);
                }
                else
                {
                    logMessage.AppendLine("No user-defined file found for: " + csUserPath);
                }

                // Start the compilation
                var compiler = new CSharpCodeProvider();
                var parameters = new CompilerParameters();
                parameters.GenerateInMemory = false;
                parameters.OutputAssembly = dllPath;
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

                var compileResult = compiler.CompileAssemblyFromFile(parameters, csList.ToArray());

                if (compileResult.Errors.Cast<CompilerError>().Any(x => x.ErrorNumber=="CS0042"))
                {
                    // When the .pdb files are loaded by Visual Studio, we cannot generate Debuginfo
                    // http://msdn.microsoft.com/de-de/library/82h240ac(v=vs.90).aspx
                    parameters.IncludeDebugInformation = false;
                    compileResult = compiler.CompileAssemblyFromFile(parameters, csList.ToArray());
                }

                if (compileResult.Errors.Count == 0)
                {
                    var setup = new AppDomainSetup()
                    {
                        ApplicationBase = workspacePath,
                        PrivateBinPath = workspacePath,
                        ConfigurationFile = null
                    };

                    var appDomain = AppDomain.CreateDomain("Runner", null, setup);
                    var type = (Simulation)appDomain.CreateInstanceAndUnwrap(
                        "ModellMeister",
                        "ModellMeister.Runner.Simulation");

                    type.Settings = new SimulationSettings()
                    {
                        SimulationTime = TimeSpan.FromSeconds(10),
                        TimeInterval = TimeSpan.FromSeconds(0.1)
                    };

                    try
                    {
                        var result = type.LoadAndStartFromLibrary("modelbased.dll");
                        
                        var resultBuilder = new StringBuilder();
                        resultBuilder.AppendLine("First 10 results");
                        foreach (var line in result.Take(10))
                        {
                            var komma = string.Empty;
                            foreach (var v in line)
                            {
                                resultBuilder.Append(komma);
                                resultBuilder.Append(v.ToString());
                                komma = ", ";
                            }

                            resultBuilder.AppendLine();
                        }

                        File.WriteAllText(resultPath, resultBuilder.ToString());

                        var resultWindow = new ResultWindow();
                        resultWindow.Results = result;
                        resultWindow.ShowDialog();
                    }
                    catch (Exception exc)
                    {
                        logMessage.AppendLine("Unhandled exception: " + exc.ToString());
                    }

                    AppDomain.Unload(appDomain);
                }
                else
                {
                    foreach (var error in compileResult.Errors)
                    {
                        logMessage.AppendLine("Compile Error: " + error.ToString());
                    }
                }
            }
            catch (Exception exc)
            {
                logMessage.AppendLine(exc.ToString());
            }
            finally
            {
                this.txtLog.Text = logMessage.ToString();
            }
        }

        private void btnSimulateSource_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
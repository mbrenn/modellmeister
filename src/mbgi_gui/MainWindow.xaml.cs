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

            this.AddMessage("Model Based Source Generator and Executor is started");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtWorkspacePath.Text = WorkspaceLogic.WorkspacePath;
            this.txtNameOfFiles.Text = currentFilename;

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
        
        private async void btnGenerateSource_Click(object sender, RoutedEventArgs e)
        {
            currentFilename = this.txtNameOfFiles.Text;

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

                this.AddMessage("C#-file Generated: " + csPath);
                this.AddMessage("MBGI-file Generated: " + csMbgiPath);

                if (File.Exists(csUserPath))
                {
                    csList.Add(csUserPath);
                    this.AddMessage("User-defined file found for: " + csUserPath);
                }
                else
                {
                    this.AddMessage("No user-defined file found for: " + csUserPath);
                }

                var dllCompiler = new Mb2DllCompiler();
                var compileResult = await dllCompiler.CompileSourceCode(workspacePath, csList, dllPath);

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
                        this.AddMessage("Running simulation");

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
                        this.AddMessage("Unhandled exception: " + exc.ToString());
                    }

                    AppDomain.Unload(appDomain);
                }
                else
                {
                    foreach (var error in compileResult.Errors)
                    {
                        this.AddMessage("Compile Error: " + error.ToString());
                    }
                }
            }
            catch (Exception exc)
            {
                this.AddMessage(exc.ToString());
            }
        }

        private void btnSimulateSource_Click(object sender, RoutedEventArgs e)
        {
        }

        public void ClearMessages()
        {
            this.txtLog.Text = string.Empty;
        }

        public void AddMessage(string message)
        {
            this.txtLog.Text = "[" + DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss") + "]: " + message + "\r\n" + this.txtLog.Text;
        }
    }
}
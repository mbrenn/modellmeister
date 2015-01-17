using BurnSystems.Logger;
using mbgi_gui.Logic;
using mbgi_gui.Models;
using Microsoft.CSharp;
using ModellMeister;
using ModellMeister.Logic;
using ModellMeister.Runner;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
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
        private static ClassLogger log = new ClassLogger(typeof(MainWindow));

        private NewFileModel modelFileModel;

        public MainWindow()
        {
            InitializeComponent();

            this.AddMessage("Model Based Source Generator and Executor is started");
            this.modelFileModel = new NewFileModel()
            {
                WorkspacePath = WorkspaceLogic.WorkspacePath,
                Filename = "modelbased"
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.LoadContent(true);
        }

        private void LoadContent(bool loadFromFile)
        {
            this.txtWorkspacePath.Text = "Workspace: \r\n" + this.modelFileModel.WorkspacePath;
            this.txtNameOfFiles.Text = "Workspace: \r\n" + this.modelFileModel.Filename;

            if (loadFromFile)
            {
                var workspacePath = this.modelFileModel.WorkspacePath;
                var csMbgiPath = Path.Combine(workspacePath, this.modelFileModel.Filename + ".mbgi");
                if (File.Exists(csMbgiPath))
                {
                    this.txtMBGISource.Text = File.ReadAllText(csMbgiPath);
                }
                else
                {
                    this.txtMBGISource.Text = string.Empty;
                }

                var csUserPath = Path.Combine(workspacePath, this.modelFileModel.Filename + ".user.cs");
                if (File.Exists(csUserPath))
                {
                    this.txtUserCs.Text = File.ReadAllText(csUserPath);
                }
                else
                {
                    this.txtUserCs.Text = string.Empty;
                }
            }
        }

        private void RunSimulationInAppDomain(string workspacePath, string dllName)
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
                var simulationResult = type.LoadAndStartFromLibrarySync(dllName);
                
                var resultWindow = new ResultWindow();
                resultWindow.Results = new ReportLogic(simulationResult);
                resultWindow.ShowDialog();
            }
            catch (Exception exc)
            {
                this.AddMessage("Unhandled exception: " + exc.ToString());
            }

            AppDomain.Unload(appDomain);
        }

        private string CreateAndGetWorkspace()
        {
            var workspacePath = this.modelFileModel.WorkspacePath;
            Environment.CurrentDirectory = workspacePath;
            if (!Directory.Exists(workspacePath))
            {
                Directory.CreateDirectory(workspacePath);
            }

            // Copies the library
            try
            {
                File.Copy(
                    Path.Combine(
                        Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                        "ModellMeister.Library.dll"),
                    Path.Combine(workspacePath, "ModellMeister.Library.dll"),
                    true);

                Mb2DllCompiler.CopyAssemblies(workspacePath);
            }
            catch
            {
                log.Message("Could not copy ModellMeister.Library, but we still continue");
            }

            return workspacePath;
        }

        public void ClearMessages()
        {
            this.txtLog.Text = string.Empty;
        }

        public void AddMessage(string message)
        {
            this.txtLog.Text = "[" + DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss") + "]: " + message + "\r\n" + this.txtLog.Text;
        }

        private async void btnRunSimulation_Click(object sender, RoutedEventArgs e)
        {
            var currentFilename = this.modelFileModel.Filename;

            try
            {
                var workspacePath = this.CreateAndGetWorkspace();
                StringBuilder generatedSource;

                List<string> importedAssemblies;

                // Gets the source code
                using (var sourceReader = new StringReader(this.txtMBGISource.Text))
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
                var csPath = Path.Combine(workspacePath, currentFilename + ".cs");
                var csMbgiPath = Path.Combine(workspacePath, currentFilename + ".mbgi");
                var dllPath = Path.Combine(workspacePath, currentFilename + ".dll");
                var csUserPath = Path.Combine(workspacePath, currentFilename + ".user.cs");
                var resultPath = Path.Combine(workspacePath, currentFilename + ".result.txt");
                csList.Add(csPath);

                File.WriteAllText(csMbgiPath, this.txtMBGISource.Text);
                File.WriteAllText(csUserPath, this.txtUserCs.Text);
                File.WriteAllText(csPath, generatedSource.ToString());

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
                dllCompiler.AddLibraries(importedAssemblies);
                var compileResult = await dllCompiler.CompileSourceCode(workspacePath, csList, dllPath);

                if (compileResult.Errors.Count == 0)
                {
                    this.AddMessage("Running simulation");
                    this.RunSimulationInAppDomain(workspacePath, currentFilename + ".dll");
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

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new NewDialog();
            dlg.Owner = this;
            dlg.Model = this.modelFileModel;
            if (dlg.ShowDialog() == true)
            {
                this.LoadContent(true);
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.RestoreDirectory = true;
            dlg.Filter = "MBGI-File|*.mbgi";
            dlg.InitialDirectory = this.modelFileModel.WorkspacePath;

            if (dlg.ShowDialog() == true)
            {
                this.modelFileModel.WorkspacePath = Path.GetDirectoryName(dlg.FileName);
                this.modelFileModel.Filename = Path.GetFileNameWithoutExtension(dlg.FileName);

                this.LoadContent(true);
            }
        }

        private void btnLoadExamples_Click(object sender, RoutedEventArgs e)
        {
            var exampleDlg = new ExampleDialog();
            exampleDlg.Owner = this;

            if (exampleDlg.ShowDialog() == true)
            {
                var selectedExample = exampleDlg.SelectedExample;
                if (selectedExample != null)
                {
                    // Load...
                    this.txtMBGISource.Text = selectedExample.MbgiFile;
                    this.txtUserCs.Text = selectedExample.CsFile;
                    this.modelFileModel.WorkspacePath = WorkspaceLogic.WorkspacePath;
                    this.modelFileModel.Filename = selectedExample.Name;

                    this.LoadContent(false);
                }
            }
        }

        private void btnOpenWorkspace_Click(object sender, RoutedEventArgs e)
        {
            var workSpacePath = this.CreateAndGetWorkspace();
            Process.Start(workSpacePath);
        }
    }
}
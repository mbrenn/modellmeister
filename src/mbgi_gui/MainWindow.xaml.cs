using BurnSystems.Logger;
using mbgi_gui.Dialogs;
using mbgi_gui.Logic;
using mbgi_gui.Models;
using ModellMeister;
using ModellMeister.Compiler;
using ModellMeister.Interfaces;
using ModellMeister.Logic;
using ModellMeister.Runner;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace mbgi_gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ILogSink
    {
        private static ClassLogger log = new ClassLogger(typeof(MainWindow));

        private SimulationSettings simulationSettings = new SimulationSettings();

        private MbgiWorkspaceCompiler workSpace;

        private GuiSettings guiSettings;

        /// <summary>
        /// Stores the filesystem watcher being used to update the content
        /// </summary>
        private FileSystemWatcher watcher = null;

        public MainWindow()
        {
            InitializeComponent();

            this.workSpace = new MbgiWorkspaceCompiler(this);
            this.AddMessageToLog("Model Based Source Generator and Executor is started");

            if (string.IsNullOrEmpty(ModellMeisterSettings.Default.LastWorkPath))
            {
                this.txtWorkspacePath.Text = WorkspaceLogic.DefaultWorkspacePath;
            }
            else
            {
                this.txtWorkspacePath.Text = ModellMeisterSettings.Default.LastWorkPath;
            }
            
            this.guiSettings = new GuiSettings()
            {
                WorkspacePath = WorkspaceLogic.DefaultWorkspacePath
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.SwitchWorkPath();
        }

        private async void btnCompile_Click(object sender, RoutedEventArgs e)
        {
            if (await this.Compile() != null)
            {
                MessageBox.Show("Compilation finished.");
            }
            else
            {
                MessageBox.Show("Compilation failed.");
            }
        }

        private async void btnRunRealtimeSimulation_Click(object sender, RoutedEventArgs e)
        {
            this.simulationSettings.Synchronous = true;

            await StartSimulation();
        }

        private async void btnRunSimulation_Click(object sender, RoutedEventArgs e)
        {
            this.simulationSettings.Synchronous = false;

            await StartSimulation();
        }

        private void btnOpenWorkspace_Click(object sender, RoutedEventArgs e)
        {
            string workSpacePath = this.guiSettings.CurrentMbgiFilePath;
            if (string.IsNullOrEmpty(workSpacePath))
            {
                workSpacePath = this.guiSettings.WorkspacePath;
            }

            if (Directory.Exists(workSpacePath))
            {
                var args = string.Format("/root, \"{0}\"", workSpacePath);
                var pfi = new ProcessStartInfo("Explorer.exe", args);
                Process.Start(pfi);
            }
            else
            {
                var args = string.Format("/select, \"{0}\"", workSpacePath);
                var pfi = new ProcessStartInfo("Explorer.exe", args);
                Process.Start(pfi);
            }
        }

        private void btnSimulationSetting_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SimulationSettingsDlg();
            dlg.Owner = this;
            dlg.DataContext = this.simulationSettings;
            if (dlg.ShowDialog() == true)
            {
                // Nothing to do here, properties are directly updated
            }
        }

        private void txtSwitchPath_Click(object sender, RoutedEventArgs e)
        {
            this.SwitchWorkPath();
        }

        private void lstFiles_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.SaveCurrentFileIfNecessary();
            this.PutFileContentToEditor();
        }

        /// <summary>
        /// Puts the file content to the editor and overwrites possible
        /// changes which were not stored into the file
        /// </summary>
        private void PutFileContentToEditor()
        {
            var selectedItem = this.lstFiles.SelectedItem;
            if (selectedItem != null)
            {
                var completePath = Path.Combine(this.guiSettings.WorkspacePath, selectedItem.ToString());
                this.guiSettings.CurrentMbgiFilePath = completePath;
                this.txtMBGISource.Text = File.ReadAllText(completePath);
            }
        }

        private bool PrepareFiles()
        {
            var filePath = this.guiSettings.CurrentMbgiFilePath;
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            File.WriteAllText(filePath, this.txtMBGISource.Text);
            return true;
        }

        /// <summary>
        /// Switches the work path
        /// </summary>
        private void SwitchWorkPath()
        {
            this.SaveCurrentFileIfNecessary();
            
            if (this.watcher != null)
            {
                this.watcher.EnableRaisingEvents = false;
                this.watcher.Dispose();
                this.watcher = null;
            }
            
            var directoryPath = this.txtWorkspacePath.Text;

            this.watcher = new FileSystemWatcher(directoryPath);
            this.watcher.Changed += OnFileIsChanged;
            this.watcher.Renamed += OnFileIsChanged;
            this.watcher.Renamed += OnDirectoryChanged;
                
            this.watcher.Deleted += OnDirectoryChanged;
            this.watcher.Created += OnDirectoryChanged;
            this.watcher.EnableRaisingEvents = true;
            if (!Directory.Exists(directoryPath))
            {
                MessageBox.Show("The given path does not exist.");
                return;
            }

            this.guiSettings.WorkspacePath = directoryPath;

            this.PopulateFileList(directoryPath);
            ModellMeisterSettings.Default.LastWorkPath = directoryPath;
            ModellMeisterSettings.Default.Save();
        }

        private void PopulateFileList(string directoryPath)
        {
            // Loads the switch path
            var files = Directory.GetFiles(directoryPath)
                .Where(x => x.EndsWith(".mbgi") || x.EndsWith(".cs"))
                .Select(x => Path.GetFileName(x));
            this.lstFiles.ItemsSource = files;
        }

        private void OnDirectoryChanged(object sender, FileSystemEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
                {
                    var directoryPath = this.guiSettings.WorkspacePath;
                    this.PopulateFileList(directoryPath);
                });
        }

        /// <summary>
        /// This method is called, when the file was changed
        /// </summary>
        /// <param name="sender">Sender being used</param>
        /// <param name="e">Arguments of the event</param>
        private void OnFileIsChanged(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath == this.guiSettings.CurrentMbgiFilePath)
            {
                Thread.Sleep(50);

                // Run asynchronously to prevent deadlock
                Task.Run(() =>
                    {
                        this.Dispatcher.Invoke(new Action(() =>
                            this.PutFileContentToEditor()));
                    });
            }
        }

        /// <summary>
        /// Compiles the current file and returns the compilation result
        /// </summary>
        /// <returns></returns>
        private async Task<MbgiCompilationResult> Compile()
        {
            var filePath = this.guiSettings.CurrentMbgiFilePath;

            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    MessageBox.Show("No files was selected");
                    return null;
                }

                if (!this.PrepareFiles())
                {
                    MessageBox.Show("Files could not be prepared");
                    return null;
                }

                if (Path.GetExtension(filePath) != ".mbgi")
                {
                    MessageBox.Show("Only .mbgi files can be started");
                    return null;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            return await this.workSpace.CompileOnMbgiFile(filePath);
        }

        /// <summary>
        /// Starts the simulation and creates all the necessary windows
        /// </summary>
        /// <returns>The task being used to run</returns>
        private async Task StartSimulation()
        {
            var compileResult = await this.Compile();
            if (compileResult != null)
            {
                this.AddMessageToLog("Running simulation");

                try
                {
                    var client = new SimulationClient(this.simulationSettings);

                    var dlg = new SimulationWindow(client);
                    dlg.Owner = this;
                    dlg.Show();
                    client.Stepped += (x, y) => dlg.RefreshData(false);
                    client.Finished += (x, y) => dlg.RefreshData(true);
                    await client.RunSimulationInAppDomain(compileResult.PathToAssembly);
                }
                catch (Exception exc)
                {
                    this.AddMessageToLog("Unhandled exception: " + exc.ToString());
                }
            }
        }

        /// <summary>
        /// Saves the current file being opened
        /// </summary>
        /// <returns>true, if one file has been saved</returns>
        private bool SaveCurrentFileIfNecessary()
        {
            return this.PrepareFiles();
        }

        #region Logging in the window

        public void ClearLogMessages()
        {
            this.txtLog.Text = string.Empty;
        }

        public void AddMessageToLog(string message)
        {
            this.txtLog.Text = "[" + DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss") + "]: " + message + "\r\n" + this.txtLog.Text;
        }

        #endregion

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://1drv.ms/1J4YEsw");
        }

        private void btnExamples_Click(object sender, RoutedEventArgs e)
        {
            var examplePath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                "examples");
            this.txtWorkspacePath.Text = examplePath;

            this.SwitchWorkPath();
        }

        private void btnWorkspace_Click(object sender, RoutedEventArgs e)
        {
            this.txtWorkspacePath.Text = WorkspaceLogic.DefaultWorkspacePath;

            this.SwitchWorkPath();
        }
    }
}
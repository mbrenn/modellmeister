﻿using BurnSystems.Logger;
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
using System.Text;
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

        public MainWindow()
        {
            InitializeComponent();

            this.workSpace = new MbgiWorkspaceCompiler(this);
            this.AddMessageToLog("Model Based Source Generator and Executor is started");

            this.txtWorkspacePath.Text = WorkspaceLogic.DefaultWorkspacePath;
            this.guiSettings = new GuiSettings()
            {
                WorkspacePath = WorkspaceLogic.DefaultWorkspacePath
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.SwitchWorkPath();
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
            var workSpacePath = this.guiSettings.WorkspacePath;
            Process.Start(workSpacePath);
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
            var directoryPath = this.txtWorkspacePath.Text;
            if (!Directory.Exists(directoryPath))
            {
                MessageBox.Show("The given path does not exist.");
            }

            this.guiSettings.WorkspacePath = directoryPath;

            // Loads the switch path
            var files = Directory.GetFiles(directoryPath)
                .Where(x => x.EndsWith(".mbgi") || x.EndsWith(".cs"))
                .Select(x => Path.GetFileName(x));
            this.lstFiles.ItemsSource = files;
        }

        /// <summary>
        /// Starts the simulation and creates all the necessary windows
        /// </summary>
        /// <returns>The task being used to run</returns>
        private async Task StartSimulation()
        {
            var filePath = this.guiSettings.CurrentMbgiFilePath;

            try
            {
                if (!this.PrepareFiles())
                {
                    MessageBox.Show("Files could not be prepared");
                    return;
                }

                if (Path.GetExtension(filePath) != ".mbgi")
                {
                    MessageBox.Show("Only .mbgi files can be started");
                    return;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            var compileResult = await this.workSpace.CompileOnMbgiFile(filePath);
            if (compileResult != null)
            {
                this.AddMessageToLog("Running simulation");

                try
                {
                    var client = new SimulationClient(this.simulationSettings);

                    var dlg = new SimulationWindow(client);
                    dlg.Owner = this;
                    dlg.Show();
                    client.Stepped += (x, y) => dlg.RefreshData();
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
    }
}
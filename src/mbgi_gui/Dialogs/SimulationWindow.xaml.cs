using mbgi_gui.Models;
using ModellMeister.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace mbgi_gui.Dialogs
{
    /// <summary>
    /// Interaction logic for WatchlistWindow.xaml
    /// </summary>
    public partial class SimulationWindow : Window
    {
        private SimulationClient client;

        public SimulationWindow(SimulationClient client)
        {
            this.client = client;
            this.InitializeComponent();
            this.model = new WatchModel(client);

            this.lstWatchItems.ItemsSource = model.Items;
        }

        private WatchModel model;

        /// <summary>
        /// Refreshes the complete data
        /// </summary>
        /// <returns>The refreshed data</returns>
        public void RefreshData()
        {
            this.Dispatcher.Invoke(() =>
                {
                    this.lstWatchItems.ItemsSource = null;
                    this.lstWatchItems.ItemsSource = model.Items;
                });
        }

        private void AddWatch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtWatchAdd.Text))
            {
                return;
            }

            if (!this.client.Server.AddChannel(this.txtWatchAdd.Text))
            {
                MessageBox.Show("The port " + this.txtWatchAdd.Text + " was not found");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtWatchAdd.Focus();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            this.client.Server.Pause();
        }

        private void btnResume_Click(object sender, RoutedEventArgs e)
        {
            this.client.Server.Resume();
        }
    }
}

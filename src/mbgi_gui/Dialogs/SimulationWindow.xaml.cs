using mbgi_gui.Models;
using ModellMeister.Runner;
using OxyPlot;
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

        private TimeSpan diagramUpdateRate = TimeSpan.FromSeconds(0.2);
        private DateTime lastUpdate = DateTime.MinValue;

        /// <summary>
        /// Refreshes the complete data. Slow, but working
        /// </summary>
        /// <param name="forceRefreshOfDiagram">true, if the diagram needs to be refreshed</param>
        /// <returns>The refreshed data</returns>
        public void RefreshData(bool forceRefreshOfDiagram)
        {
            this.Dispatcher.Invoke(() =>
                {
                    this.lstWatchItems.ItemsSource = null;
                    this.lstWatchItems.ItemsSource = this.model.Items;

                    // Only update the graph, when time is done
                    if (forceRefreshOfDiagram || 
                        DateTime.Now - lastUpdate > diagramUpdateRate)
                    {
                        lastUpdate = DateTime.Now;
                        var clientResult = this.client.SimulationResult.Result;
                        if (clientResult.Count > 0)
                        {
                            var oxyModel = new PlotModel();
                            var maxLines = clientResult.Max(x => x.Values.Count());
                            var oxySeries = new List<OxyPlot.Series.LineSeries>();
                            for (var n = 0; n < maxLines; n++)
                            {
                                var newSeries = new OxyPlot.Series.LineSeries();
                                oxySeries.Add(newSeries);
                                oxyModel.Series.Add(newSeries);
                            }

                            foreach (var state in clientResult)
                            {
                                var n = 0;
                                foreach (var v in state.Values)
                                {
                                    oxySeries[n].Points.Add(new DataPoint(state.AbsoluteTime.TotalSeconds, Convert.ToDouble(v)));
                                    n++;
                                }
                            }

                            this.View.Model = oxyModel;
                        }
                    }
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
            if (this.client != null && this.client.Server != null)
            {
                this.client.Server.Pause();
            }
        }

        private void btnResume_Click(object sender, RoutedEventArgs e)
        {
            if (this.client != null && this.client.Server != null)
            {
                this.client.Server.Resume();
            }
        }
    }
}

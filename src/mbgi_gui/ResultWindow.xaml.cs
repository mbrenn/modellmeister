using ModellMeister.Logic;
using OxyPlot;
using OxyPlot.Series;
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

namespace mbgi_gui
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public ReportLogic Results
        {
            set
            {
                var model = new PlotModel();

                foreach (var series in value.LineSeries)
                {
                    var oxySeries = new OxyPlot.Series.LineSeries();

                    var n = 0;
                    foreach (var v in series.Values)
                    {
                        oxySeries.Points.Add(new DataPoint(n, v));
                        n++;
                    }

                    model.Series.Add(oxySeries);
                }

                this.View.Model = model;
            }
        }

        public ResultWindow()
        {
            InitializeComponent();
        }
    }
}

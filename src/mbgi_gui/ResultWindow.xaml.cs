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
        public IList<object[]> Results
        {
            set
            {
                var model = new PlotModel();
                var n = 0;
                var series = new OxyPlot.Series.LineSeries();
                model.Series.Add(series);
                foreach (var l in value)
                {
                    series.Points.Add(new DataPoint(n, (double)(l[0])));
                    n++;
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

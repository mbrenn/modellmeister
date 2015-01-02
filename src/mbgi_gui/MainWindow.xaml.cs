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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace mbgi_gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.AddLog("Model Based Source Generator and Executor is started");
        }

        public void AddLog(string line)
        {
            this.txtLog.Text = "[" + DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss") + "]: " + line + "\r\n" + this.txtLog.Text;
        }
    }
}

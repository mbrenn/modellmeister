using mbgi_gui.Models;
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
    /// Interaction logic for NewDialog.xaml
    /// </summary>
    public partial class NewDialog : Window
    {
        public NewDialog()
        {
            InitializeComponent();
        }

        public GuiSettings Model
        {
            get;
            set;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this.Model;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

        }
    }
}

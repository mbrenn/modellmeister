using mbgi_gui.Logic;
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
    /// Interaction logic for ExampleDialog.xaml
    /// </summary>
    public partial class ExampleDialog : Window
    {
        /// <summary>
        /// Gets the selected example
        /// </summary>
        public ExampleItem SelectedExample
        {
            get
            {
                return this.lstExamples.SelectedItem as ExampleItem;
            }
        }

        public ExampleDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = Examples.LoadExamples();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void ListBoxItem_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

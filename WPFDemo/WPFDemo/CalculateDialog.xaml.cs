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
using WPFDemo.ViewModel;
namespace WPFDemo
{
    /// <summary>
    /// Interaction logic for CalculateDialog.xaml
    /// </summary>
    public partial class CalculateDialog : Window
    {
        public CalculateDialog()
        {
            InitializeComponent();
            this.DataContext = new CalculateViewModel();
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}

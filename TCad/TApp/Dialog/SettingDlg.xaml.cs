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
using TApp.ViewModel;

namespace TApp.Dialog
{
    /// <summary>
    /// Interaction logic for SettingDlg.xaml
    /// </summary>
    public partial class SettingDlg : Window
    {
        private SettingDlgViewModel _settingDlgVM;

        public int GetRed()
        {
            int red =  Convert.ToInt32(_settingDlgVM.RedText);
            return red;
        }

        public int GetGreen()
        {
            int green = Convert.ToInt32(_settingDlgVM.GreenText);
            return green;
        }

        public int GetBlue()
        {
            int blue = Convert.ToInt32(_settingDlgVM.BlueText);
            return blue;
        }

        public SettingDlg()
        {
            InitializeComponent();
            _settingDlgVM = new SettingDlgViewModel();
            this.DataContext = _settingDlgVM;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}

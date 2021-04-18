using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace BTea
{
    /// <summary>
    /// Interaction logic for BSetting.xaml
    /// </summary>
    public partial class BSetting : Window
    {
        public BSetting()
        {
            InitializeComponent();
        }

        private void Button_Click_Ok(object sender, RoutedEventArgs e)
        {
            BSettingVM _settingVM = DataContext as BSettingVM;

            if (_settingVM != null)
            {
                string lPrice = _settingVM.LPrice.ToString();

                Configuration oConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                oConfig.AppSettings.Settings["lprice"].Value = lPrice;
                oConfig.Save(ConfigurationSaveMode.Modified, true);
                ConfigurationManager.RefreshSection("appSettings");
            }

            DialogResult = true;
            this.Close();
        }

        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}

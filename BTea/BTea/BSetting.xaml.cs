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
                string sName = _settingVM.ShopName;
                string sPhone = _settingVM.ShopPhone;
                string sAddress = _settingVM.ShopAddress;
                string sFace = _settingVM.ShopFacebook;

                Configuration bConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                bConfig.AppSettings.Settings["lprice"].Value = lPrice;
                bConfig.AppSettings.Settings["shopname"].Value = sName;
                bConfig.AppSettings.Settings["shopphone"].Value = sPhone;
                bConfig.AppSettings.Settings["shopaddress"].Value = sAddress;
                bConfig.AppSettings.Settings["shopface"].Value = sFace;

                bConfig.Save(ConfigurationSaveMode.Modified, true);
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

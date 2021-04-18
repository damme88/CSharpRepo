using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TApp.Base;

namespace BTea
{
    class BSettingVM : TBaseVM
    {
        public BSettingVM()
        {
            string path = Environment.CurrentDirectory;
            path += "\\img_data\\";
            ImgFolderPath = path;
            SettingShowFolderPathCmd = new RelayCommand(new Action<object>(ShowImgPathFolder));

            string sPrice = ConfigurationManager.AppSettings["lprice"].ToString();
            try
            {
                _lPrice = Convert.ToDouble(sPrice);
            }
            catch
            {
                _lPrice = 10000;
            }
        }

        private double _lPrice;
        public double LPrice
        {
            get { return _lPrice; }
            set { _lPrice = value;
                OnPropertyChange("LPrice");
            }
        }

        public string ImgFolderPath { set; get; }
        public RelayCommand SettingShowFolderPathCmd { set; get; }

        public void ShowImgPathFolder(object obj)
        {
            bool isValid = true;
            if (ImgFolderPath != "")
            {
                if (Directory.Exists(ImgFolderPath) == true)
                {
                    Process.Start("explorer.exe", ImgFolderPath);
                }
                else
                {
                    isValid = false;
                }
            }
            else
            {
                isValid = false;
            }

            if (isValid == false)
            {
                MessageBox.Show("Đường dẫn không tồn tại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

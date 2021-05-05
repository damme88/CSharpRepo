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
            string path = Environment.CurrentDirectory + "\\img_data\\";
            ImgFolderPath = path;
            SettingShowFolderPathCmd = new RelayCommand(new Action<object>(ShowImgPathFolder));

            string sPrice = ConfigurationManager.AppSettings["lprice"].ToString();
            try
            {
                _lPrice = TConst.ConvertMoney(sPrice);
            }
            catch
            {
                _lPrice = 10000;
            }

            _shopName = ConfigurationManager.AppSettings["shopname"].ToString();
            _shopPhone = ConfigurationManager.AppSettings["shopphone"].ToString();
            _shopAddress = ConfigurationManager.AppSettings["shopaddress"].ToString();
            _shopFace = ConfigurationManager.AppSettings["shopface"].ToString();
        }

        private string _shopName;
        public string ShopName
        {
            get { return _shopName; }
            set { _shopName = value;  OnPropertyChange("ShopName"); }
        }

        private string _shopPhone;
        public string ShopPhone
        {
            get { return _shopPhone; }
            set { _shopPhone = value; OnPropertyChange("ShopPhone"); }
        }

        private string _shopAddress;
        public string ShopAddress
        {
            get { return _shopAddress; }
            set { _shopAddress = value; OnPropertyChange("ShopAddress"); }
        }

        private string _shopFace;
        public string ShopFacebook
        {
            get { return _shopFace; }
            set { _shopFace = value; OnPropertyChange("ShopFacebook"); }
        }

        private int _lPrice;
        public string LPrice
        {
            get
            {
                return _lPrice.ToString(TConst.K_MONEY_FORMAT);
            }
            set
            {
                int iVal = TConst.ConvertMoney(value);
                _lPrice = iVal;
                OnPropertyChange("LPrice");
            }
        }

        public string ImgFolderPath { set; get; }
        public RelayCommand SettingShowFolderPathCmd { set; get; }

        public void ShowImgPathFolder(object obj)
        {
            if (ImgFolderPath != "")
            {
                if (Directory.Exists(ImgFolderPath) == true)
                {
                    Process.Start("explorer.exe", ImgFolderPath);
                    return;
                }
            }

            TConst.MsgError("Đường dẫn không tồn tại.");
        }
    }
}

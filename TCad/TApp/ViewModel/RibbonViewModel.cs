using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TApp.Base;
using TApp.Dialog;

namespace TApp.ViewModel
{
    class BackgrounColor
    {
        public int _red;
        public int _green;
        public int _blue;
    }

    class RibbonViewModel : TBaseVM
    {
        public RibbonViewModel()
        {
            NewCmd = new RelayCommand(new Action<object>(ImpNew));
            OpenCmd = new RelayCommand(new Action<object>(ImpOpen));
            SaveCmd = new RelayCommand(new Action<object>(ImpSave));
            SettingCmd = new RelayCommand(new Action<object>(ImpSetting));
            _bkgnColor = new BackgrounColor();
        }

        private BackgrounColor _bkgnColor;

        public RelayCommand NewCmd { set; get; }
        public RelayCommand OpenCmd { set; get; }
        public RelayCommand SaveCmd { set; get; }
        public RelayCommand SettingCmd { set; get; }

        public BackgrounColor BkgnColor
        {
            get { return _bkgnColor; }
        }

        public void ImpNew(object obj)
        {
            MessageBox.Show("Hello WPF");
        }

        public void ImpOpen(object obj)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.Filter = "Text Files (*.txt)|*.txt|Stl Files (*.stl)|*.stl|All Files(*.*)|*.*";
            if (fileDlg.ShowDialog() == true)
            {
                string str = fileDlg.FileName;
                int a = 5;
                a++;
            }
        }

        public void ImpSave(object obj)
        {

        }

        public void ImpSetting(object obj)
        {
            SettingDlg setting_dlg = new SettingDlg();
            if (setting_dlg.ShowDialog() == true)
            {
                _bkgnColor._red = setting_dlg.GetRed();
                _bkgnColor._green = setting_dlg.GetGreen();
                _bkgnColor._blue = setting_dlg.GetBlue();

                WrapApp.Instance.WrappGl.UpdateColorBkgn(_bkgnColor._red, _bkgnColor._green, _bkgnColor._blue);
            }
        }
    }
}

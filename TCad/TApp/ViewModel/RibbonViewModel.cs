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
        public enum CommandType
        {
            CMD_NONE = 0,
            CMD_LINE_FRM = 1,
        }

        public RibbonViewModel(Action parentAction)
        {
            ExitCmd = new RelayCommand(new Action<object>(DoExit));
            NewCmd = new RelayCommand(new Action<object>(ImpNew));
            OpenCmd = new RelayCommand(new Action<object>(DoOpen));
            SaveCmd = new RelayCommand(new Action<object>(ImpSave));
            SettingCmd = new RelayCommand(new Action<object>(DoSetting));
            _bkgnColor = new BackgrounColor();
            LineCmd = new RelayCommand(new Action<object>(DrawLine));

            _pMainMethod = parentAction;
            _commandType = CommandType.CMD_NONE;
        }

        #region Members

        public RelayCommand ExitCmd { set; get; }

        private BackgrounColor _bkgnColor;
        private readonly Action _pMainMethod;
        private CommandType _commandType;

        public RelayCommand NewCmd { set; get; }
        public RelayCommand OpenCmd { set; get; }
        public RelayCommand SaveCmd { set; get; }
        public RelayCommand SettingCmd { set; get; }
        public RelayCommand LineCmd { set; get; }

        #endregion

        #region PROPERTY
        public BackgrounColor BkgnColor
        {
            get { return _bkgnColor; }
        }

        public int ComdType
        {
            get { return (int)_commandType; }
        }

        #endregion

        #region FUNCTION
        public void DoExit(object obj)
        {
            ;// Application.Current.Shutdown();
        }

        public void ImpNew(object obj)
        {
            MessageBox.Show("Hello WPF");
        }

        public void DoOpen(object obj)
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

        public void DoSetting(object obj)
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

        public void DrawLine(object obj)
        {
            _commandType = CommandType.CMD_LINE_FRM;
            _pMainMethod.Invoke();
        }

        #endregion
    }
}

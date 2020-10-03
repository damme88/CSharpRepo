using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using TApp.Base;

namespace TApp.ViewModel
{
    class UserName
    {
        public UserName(string name, string age)
        {
            _name = name;
            _age = age;
        }
        public string _name;
        public string _age;

        public string Name
        {
            get { return _name; }
        }
        public string Age 
        {
            get { return _age; }
        }
    }
    class SettingDlgViewModel : TBaseVM
    {
        public RelayCommand CmdColorBox { set; get; }
        private SolidColorBrush _bkgnColor;
        public SettingDlgViewModel()
        {
            _redText = "0";
            _greenText = "0";
            _blueText = "0";
            _bkgnColor = new SolidColorBrush(Colors.Black);
            CmdColorBox = new RelayCommand(new Action<object>(OnColorBox));
            _lvItems = new List<UserName>();
            _lvItems.Add(new UserName("Pham 1", "25"));
            _lvItems.Add(new UserName("Pham 2", "27"));

        }

        string _redText;
        string _greenText;
        string _blueText;
        List<UserName> _lvItems;
        public List<UserName> LVItems
        {
            get { return _lvItems; }
            set
            {
                if (_lvItems != value)
                {
                    _lvItems = value;
                }
                OnPropertyChange("LVItems");
            }
        }
        public string RedText
        {
            get { return _redText; }
            set {
                if (_redText != value)
                {
                    _redText = value;
                    OnPropertyChange("RedText");
                    UpdateColor();
                }
            }
        }

        public string GreenText
        {
            get { return _greenText; }
            set
            {
                if (_greenText != value)
                {
                    _greenText = value;
                    OnPropertyChange("GreenText");
                    UpdateColor();
                }
            }
        }

        public string BlueText
        {
            get { return _blueText; }
            set {
                if (_blueText != value)
                {
                    _blueText = value;
                    OnPropertyChange("BlueText");
                    UpdateColor();
                }
            }
        }

        public void OnColorBox(object sender)
        {
            ColorDialog dlg = new ColorDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _bkgnColor.Color = Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B);
                int red = Convert.ToInt32(dlg.Color.R);
                int green = Convert.ToInt32(dlg.Color.G);
                int blue = Convert.ToInt32(dlg.Color.B);

                RedText = red.ToString();
                GreenText = green.ToString();
                BlueText = blue.ToString();
            }
        }

        void UpdateColor()
        {
            int red = 0;
            int green = 0;
            int blue = 0;
            try
            {
                red = Convert.ToInt32(_redText);
                green = Convert.ToInt32(_greenText);
                blue = Int32.Parse(_blueText);
            }
            catch
            {
                red = _bkgnColor.Color.R;
                green = _bkgnColor.Color.G;
                blue = _bkgnColor.Color.B;
                RedText = red.ToString();
                GreenText = green.ToString();
                BlueText = blue.ToString();
            }

            byte anpha = _bkgnColor.Color.A;
            byte bRed = Convert.ToByte(red);
            byte bGreen = Convert.ToByte(green);
            byte bBlue = Convert.ToByte(blue);
            BkgnColor.Color = Color.FromArgb(anpha, bRed, bGreen, bBlue);
        }
        public SolidColorBrush BkgnColor
        {
            get { return _bkgnColor; }
            set
            {
                if (_bkgnColor != value)
                {
                    _bkgnColor = value;
                    OnPropertyChange("BkgnColor");
                }
            }
        }
    }
}

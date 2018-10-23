using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFDemo.Base;

namespace WPFDemo
{
    class CalculateViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertiyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public RelayCommand RetBtnCommand { set; get; }



        public CalculateViewModel()
        {
            RetBtnCommand = new RelayCommand(new Action<object>(RetBtnFunction));

        }

        public string val1 = "0";
        public string Value1
        {
            get
            {
                return val1;
            }
            set
            {
                if (value != val1)
                {
                    val1 = value;
                    OnPropertiyChange("Value1");
                }
            }
        }

        public string val2 = "0";
        public string Value2
        {
            get
            {
                return val2;
            }
            set
            {
                if (value != val2)
                {
                    val2 = value;
                    OnPropertiyChange("Value2");
                }
            }
        }

        public string retval = "0";
        public string RetVal 
        {
            get
            {
                return retval;
            }
            set
            {
                if (value != retval)
                {
                    retval = value;
                    OnPropertiyChange("RetVal");
                }
            }
        }

        private string textInfo = "";
        public string TextInfo
        {
            get
            {
                return textInfo;
            }

            set
            {
                if (value != textInfo)
                {
                    textInfo = value;
                    OnPropertiyChange("TextInfo");
                }
            }
        }

        public void RetBtnFunction(object objt)
        {
            if (val1 != string.Empty && val2 != string.Empty)
            {
                double dval1 = Convert.ToDouble(val1);
                double dval2 = Convert.ToDouble(val2);

                double retVal = dval1 + dval2;
                RetVal = retVal.ToString();
            }
        }
    }
}

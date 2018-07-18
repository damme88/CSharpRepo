using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPFDemo
{
    class RadioConvertToInt : IValueConverter
    {

        public object Convert(object value, Type targetType, object param, CultureInfo cultureInfo)
        {
            int interger = (int)value;
            if (interger == int.Parse(param.ToString()))
            {
                return true;
            }
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object param, CultureInfo cultureInfo)
        {
            return param;
        }

    }
}

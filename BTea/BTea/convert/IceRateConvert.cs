﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BTea.convert
{
    class IceRateConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string sSize = "";
            string val = (string)value;
            if (val == "1")
            {
                sSize = "30%";
            }
            else if (val == "2")
            {
                sSize = "50%";
            }
            else if (val == "3")
            {
                sSize = "70%";
            }
            else
            {
                sSize = "100%";
            }

            return sSize;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

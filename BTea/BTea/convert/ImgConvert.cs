﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BTea.convert
{
    class ImgConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string path = Environment.CurrentDirectory;
            string val = (string)value;

            path += "\\img_data\\" + value + ".png";

            if (File.Exists(path))
            {
                return path;
            }
            else
            {
                path = Environment.CurrentDirectory;
                path += "\\img_data\\sample.png";
                if (File.Exists(path))
                {
                    return path;
                }
            }
            return string.Empty;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

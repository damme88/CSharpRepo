using System;
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
            int val = (int)value;
            if (val > 10)
            {
                path += "\\img_data\\" + "10_plus.png";
            }
            else
            {
                path += "\\img_data\\" + value.ToString() + ".png";
            }

            if (File.Exists(path))
            {
                return path;
            }
            return string.Empty;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

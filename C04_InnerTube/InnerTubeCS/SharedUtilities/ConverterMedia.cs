using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using SharedUtilities;

namespace SharedUtilities
{
    [ValueConversion(typeof(object), typeof(string))]
    public class ConverterMedia : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            InnerTubeVideo v = (InnerTubeVideo)value;

            if (!String.IsNullOrEmpty(v.DownloadedWmv))
            {
                return v.DownloadedWmv;
            }
            else
            {
                return v.DownloadedImage;
            }          

        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            // we don't intend this to ever be called
            return null;
        }
    }
}

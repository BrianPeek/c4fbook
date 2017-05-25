using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SharedUtilities
{
    [ValueConversion(typeof(object), typeof(string))]
    public class ConverterImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string path = (string)value;

            if (String.IsNullOrEmpty(path))
            {
                //use default image
                return @"Images\youtube.jpg";
            }
            else
            {
                return path;
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

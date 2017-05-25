using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.IO;
using SharedUtilities;
using System.Windows.Media;

namespace InnerTube
{
    [ValueConversion(typeof(object), typeof(string))]
    public class ConverterImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string path = (string)value;
            return SetImage(path);           
        }

        public static object SetImage(string imagePath)
        {
            //if the path doesn't exist
            if (String.IsNullOrEmpty(imagePath) || (!File.Exists(imagePath)))
            {
                //use default image
                return FileHelper.DefaultImage;
            }
            else
            {
                return imagePath;
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

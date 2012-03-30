using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace CoApp.Updater.Support.Converters
{
    public class UrlToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var uriString = value as string;
            if (uriString != null)
            {
                Uri ignored;
                return Uri.TryCreate(uriString, UriKind.Absolute, out ignored);
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

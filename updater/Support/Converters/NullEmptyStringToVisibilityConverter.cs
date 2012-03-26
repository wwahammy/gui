using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CoApp.Updater.Support.Converters
{
    [ValueConversion(typeof(string), typeof(Visibility))]
    public class NullEmptyStringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || (value is string && ((string)value) == String.Empty))
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

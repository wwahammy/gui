using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using CoApp.PackageManager.ViewModel;

namespace CoApp.PackageManager.Support.Converters
{
    [ValueConversion(typeof(int), typeof(bool))]
    public class GreaterThanZeroIsTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var num = value as int?;
            if (num != null)
            {
                return num > 0;
                
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using CoApp.Packaging.Client;


namespace CoApp.PackageManager.Support.Converters
{
    
    [ValueConversion(typeof(Package), typeof(string))]
    public class PrettyPackageNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var p = value as Package;
            if (p != null)
            {
                return p.GetNicestPossibleName();
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

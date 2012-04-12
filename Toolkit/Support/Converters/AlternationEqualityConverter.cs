using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace CoApp.Gui.Toolkit.Support.Converters
{
    /// <summary>
    /// http://stackoverflow.com/questions/7834987/distinct-item-template-for-first-and-last-item-in-a-listview
    /// </summary>
    public class AlternationEqualityConverter : IMultiValueConverter 
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length == 2 &&
                values[0] is int && values[1] is int)
            {
                return Equals((int)values[0], (int)values[1] + 1);
            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }
}

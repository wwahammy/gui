using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace CoApp.Gui.Toolkit.Support.Converters
{
    /// <summary>
    /// When the <see cref="IEnumerable"/> passed in is empty, returns <see cref="Visibility.Collapsed"/>, otherwise <see cref="Visibility.Visible"/>
    /// If the parameter is bool and set to true, invert when each visibility is returned.
    /// </summary>
    [ValueConversion(typeof(IEnumerable), typeof(Visibility), ParameterType = typeof(bool))]
    public class EnumerableCountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? param = null;
            if (parameter is string)
            {
                bool test;
                if (Boolean.TryParse((string)parameter, out test))
                {
                    param = test;
                }
                else
                {
                    param = null;
                }
            }

            if (value != null && value is IEnumerable)
            {
                var e = ((IEnumerable) value).Cast<object>();
                var empty = Visibility.Collapsed;
                var filled = Visibility.Visible;
                
                if (param != null)
                {
                    var b = (bool) param;
                    if (b)
                    {
                        empty = Visibility.Visible;
                        filled = Visibility.Collapsed;
                    }
                }

                return e.Any() ?  filled : empty;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

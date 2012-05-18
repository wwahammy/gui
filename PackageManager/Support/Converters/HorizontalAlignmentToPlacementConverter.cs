using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace CoApp.PackageManager.Support.Converters
{
    public class HorizontalAlignmentToPlacementConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is HorizontalAlignment)
            {
                var align = (HorizontalAlignment) value;
                if (align == HorizontalAlignment.Right)
                    return PlacementMode.Custom;
            }

            return PlacementMode.Bottom;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

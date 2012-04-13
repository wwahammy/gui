using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace CoApp.Gui.Toolkit.Support.Converters.ProgressBar
{
    public class WidthToContainerAnimationStartPositionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                var width = (double)value;
                if (width <= 180)
                    return -34;
                if (width <= 280)
                    return -50.5;

                return -63;

            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

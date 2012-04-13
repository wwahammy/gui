using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace CoApp.Gui.Toolkit.Support.Converters.ProgressBar
{
    public class WidthToContainerAnimationEndPositionConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                var width = (double)value;
                var firstPart = 0.4352*width;
                if (width <= 180)
                    return firstPart-25.731;
                if (width <= 280)
                    return firstPart+27.84;

                return firstPart+58.862;

            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

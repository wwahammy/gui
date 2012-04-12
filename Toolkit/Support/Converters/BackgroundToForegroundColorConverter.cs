using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace CoApp.Gui.Toolkit.Support.Converters
{
    /// <summary>
    /// http://stackoverflow.com/questions/6763032/how-to-pick-a-background-color-depending-on-font-color-to-have-proper-contrast
    /// </summary>
    public class BackgroundToForegroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           
            if (value != null && (value is Color || value is SolidColorBrush))
            {
                var c = new Color();
                if (value is Color)
                    c = (Color) value;
                if (value is SolidColorBrush)
                {
                    c = ((SolidColorBrush) value).Color;
                }

                
                var l = 0.2126*c.ScR + 0.7152*c.ScG + 0.0722*c.ScB;
                if (l < 0.5)
                {
                    return Brushes.White;
                }
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

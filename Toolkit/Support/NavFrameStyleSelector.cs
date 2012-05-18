using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using CoApp.Gui.Toolkit.Controls;
using CoApp.Gui.Toolkit.ViewModels;

namespace CoApp.Gui.Toolkit.Support
{
    public class NavFrameStyleSelector : StyleSelector
    {
        public override System.Windows.Style SelectStyle(object item, System.Windows.DependencyObject container)
        {
            Style style = new Style();
            style.TargetType = typeof (CoAppFrame);
            if (item != null && item is CoAppFrame && ((CoAppFrame)item).Content != null && ((CoAppFrame)item).Content is ScreenViewModel)
            {
                var screenWidth = ((ScreenViewModel) ((CoAppFrame) item).Content).ScreenWidth;
                var setter = new Setter(Grid.ColumnProperty, screenWidth == ScreenWidth.FullWidth ? 0: 1);
                style.Setters.Add(setter);

            }
            else
            {
                var setter = new Setter(Grid.ColumnProperty, 1);
                style.Setters.Add(setter);
            }

            return style;

        }
    }
}

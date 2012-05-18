using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CoApp.PackageManager.Controls
{
    public class OtherHeaderItems
    {
        public static readonly DependencyProperty OtherHeaderItemsProperty =
            DependencyProperty.RegisterAttached("OtherHeaderItems", typeof (IList), typeof (UserControl), new PropertyMetadata(default(IList)));

        public static void SetOtherHeaderItems(UIElement element, IList value)
        {
            element.SetValue(OtherHeaderItemsProperty, value);
        }

        public static IList GetOtherHeaderItems(UIElement element)
        {
            return (IList) element.GetValue(OtherHeaderItemsProperty);
        }

        
    }

}

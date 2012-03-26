using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CoApp.Updater.Support
{
    public class CanBlockProperties
    {
        public static readonly DependencyProperty CanBlockProperty =
            DependencyProperty.RegisterAttached("CanBlock", typeof(bool?), typeof(CanBlockProperties), new PropertyMetadata(default(bool?)));

        public static void SetCanBlock(UIElement element, bool? value)
        {
            element.SetValue(CanBlockProperty, value);
        }

        public static bool? GetCanBlock(UIElement element)
        {
            return (bool?) element.GetValue(CanBlockProperty);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CoApp.Gui.Toolkit.Support
{
    public static class TabControlHelper
    {
        public static readonly DependencyProperty HeaderFontSizeProperty =
            DependencyProperty.RegisterAttached("HeaderFontSize", typeof(double), typeof(TabControlHelper), new PropertyMetadata(default(double)));

        public static void SetHeaderFontSize(TabControl element, double value)
        {
            element.SetValue(HeaderFontSizeProperty, value);
        }

        [TypeConverter(typeof(LengthConverter))]
        //   [AttachedPropertyBrowsableForType(typeof(TabControl))]
        public static double GetHeaderFontSize(TabControl element)
        {
            return (double)element.GetValue(HeaderFontSizeProperty);
        }

        public static readonly DependencyProperty HeaderWidthProperty =
            DependencyProperty.RegisterAttached("HeaderWidth", typeof(double), typeof(TabControlHelper), new PropertyMetadata(default(double)));

        public static void SetHeaderWidth(TabControl element, double value)
        {
            element.SetValue(HeaderWidthProperty, value);
        }

        [TypeConverter(typeof(LengthConverter))]
        // [AttachedPropertyBrowsableForType(typeof(TabControl))]
        public static double GetHeaderWidth(TabControl element)
        {
            return (double)element.GetValue(HeaderWidthProperty);
        }


        public static readonly DependencyProperty HeaderMinWidthProperty =
            DependencyProperty.RegisterAttached("HeaderMinWidth", typeof(double), typeof(TabControlHelper), new PropertyMetadata(default(double)));



        public static void SetHeaderMinWidth(TabControl element, double value)
        {
            element.SetValue(HeaderMinWidthProperty, value);
        }

        [TypeConverter(typeof(LengthConverter))]

        public static double GetHeaderMinWidth(TabControl element)
        {
            return (double)element.GetValue(HeaderMinWidthProperty);
        }

        public static readonly DependencyProperty HeaderMaxWidthProperty =
            DependencyProperty.RegisterAttached("HeaderMaxWidth", typeof(double), typeof(TabControlHelper), new PropertyMetadata(default(double)));

        public static void SetHeaderMaxWidth(TabControl element, double value)
        {
            element.SetValue(HeaderMaxWidthProperty, value);
        }

        [TypeConverter(typeof(LengthConverter))]
        // [AttachedPropertyBrowsableForType(typeof(TabControl))]
        public static double GetHeaderMaxWidth(TabControl element)
        {
            return (double)element.GetValue(HeaderMaxWidthProperty);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace CoApp.Updater.Controls
{
    public class PercentTransform
    {
        public static readonly DependencyProperty PercentFromLeftProperty =
            DependencyProperty.RegisterAttached("PercentFromLeft", typeof (double), typeof (PercentTransform), new PropertyMetadata(default(double), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (dependencyObject is TranslateTransform && dependencyPropertyChangedEventArgs.Property.Name == "PercentFromLeft")
            {
                var transform = (TranslateTransform) dependencyObject;
                var outerWidth = (double)transform.GetValue(OuterWidthProperty);
                transform.SetValue(TranslateTransform.XProperty, outerWidth * (double)dependencyPropertyChangedEventArgs.NewValue);


            }
        }

        public static void SetPercentFromLeft(TranslateTransform element, double value)
        {
            element.SetValue(PercentFromLeftProperty, value);
        }

        public static double GetPercentFromLeft(TranslateTransform element)
        {
            return (double) element.GetValue(PercentFromLeftProperty);
        }


        public static readonly DependencyProperty OuterWidthProperty =
            DependencyProperty.RegisterAttached("OuterWidth", typeof (double), typeof (PercentTransform), new PropertyMetadata(default(double)));

        public static void SetOuterWidth(TranslateTransform element, double value)
        {
            element.SetValue(OuterWidthProperty, value);
        }

        public static double GetOuterWidth(TranslateTransform element)
        {
            return (double) element.GetValue(OuterWidthProperty);
        }
    }
}

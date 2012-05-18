using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;

namespace CoApp.PackageManager.Controls
{
    /// <summary>
    /// Don't override the itemtemplate or itemspanel. It will break things.
    /// ControlTemplate From http://stackoverflow.com/questions/2511227/how-can-a-separator-be-added-between-items-in-an-itemscontrol
    /// </summary>
    public class DelimitedControl : ItemsControl
    {
        static DelimitedControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DelimitedControl), new FrameworkPropertyMetadata(typeof(DelimitedControl)));
        }

        public static readonly DependencyProperty ItemDelimiterProperty =
            DependencyProperty.Register("ItemDelimiter", typeof (string), typeof (DelimitedControl), new PropertyMetadata(", ", PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            
        }

        public string ItemDelimiter
        {
            get { return (string)GetValue(ItemDelimiterProperty); }
            set
            {
                SetValue(ItemDelimiterProperty, value);
            }
        }

        public static readonly DependencyProperty ValueTemplateProperty =
            DependencyProperty.Register("ValueTemplate", typeof (DataTemplate), typeof (DelimitedControl), new PropertyMetadata(default(DataTemplate), ValueChaNGED ));

        private static void ValueChaNGED(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            
        }


        public DataTemplate ValueTemplate
        {
            get { return (DataTemplate) GetValue(ValueTemplateProperty); }
            set
            {
                SetValue(ValueTemplateProperty, value);
            }
        }


    }
}

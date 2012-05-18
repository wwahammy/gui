using System;
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

namespace CoApp.Gui.Toolkit.Controls
{
    public class StyleableHeaderControl : HeaderedContentControl
    {
        static StyleableHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StyleableHeaderControl), new FrameworkPropertyMetadata(typeof(StyleableHeaderControl)));
        }

        public static readonly DependencyProperty HeaderStyleProperty =
            DependencyProperty.Register("HeaderStyle", typeof (Style), typeof (StyleableHeaderControl), new PropertyMetadata(default(Style)));

        public Style HeaderStyle
        {
            get { return (Style) GetValue(HeaderStyleProperty); }
            set { SetValue(HeaderStyleProperty, value); }
        }
    }
}

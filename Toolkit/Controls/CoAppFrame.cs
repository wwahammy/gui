using System;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace CoApp.Gui.Toolkit.Controls
{
    public class CoAppFrame : TransitioningContentControl
    {


        public static readonly DependencyProperty WaitForLoadProperty =
            DependencyProperty.Register("WaitForLoad", typeof (bool), typeof (CoAppFrame), new PropertyMetadata(default(bool)));

        public bool WaitForLoad
        {
            get { return (bool) GetValue(WaitForLoadProperty); }
            set { SetValue(WaitForLoadProperty, value); }
        }


       
    }
}

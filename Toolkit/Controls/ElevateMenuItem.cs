using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoApp.Gui.Toolkit.Controls
{
    public class ElevateMenuItem : MenuItem
    {
        static ElevateMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ElevateMenuItem), new FrameworkPropertyMetadata(typeof(ElevateMenuItem)));
        }

        public static readonly DependencyProperty UnelevatedCommandProperty =
            DependencyProperty.Register("UnelevatedCommand", typeof (ICommand), typeof (ElevateMenuItem), new PropertyMetadata(default(ICommand)));

        public ICommand UnelevatedCommand
        {
            get { return (ICommand) GetValue(UnelevatedCommandProperty); }
            set { SetValue(UnelevatedCommandProperty, value); }
        }

        public static readonly DependencyProperty ElevatedCommandProperty =
            DependencyProperty.Register("ElevatedCommand", typeof (ICommand), typeof (ElevateMenuItem), new PropertyMetadata(default(ICommand)));

        public ICommand ElevatedCommand
        {
            get { return (ICommand) GetValue(ElevatedCommandProperty); }
            set { SetValue(ElevatedCommandProperty, value); }
        }

        public static readonly DependencyProperty MustElevateProperty =
            DependencyProperty.Register("MustElevate", typeof (bool), typeof (ElevateMenuItem), new PropertyMetadata(default(bool)));

        public bool MustElevate
        {
            get { return (bool) GetValue(MustElevateProperty); }
            set { SetValue(MustElevateProperty, value); }
        }

    }
}

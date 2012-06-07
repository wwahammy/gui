using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoApp.Gui.Toolkit.Controls
{
    /// <summary>
    /// Interaction logic for PopUpControl.xaml
    /// </summary>
    public partial class PopUpControl
    {
        public PopUpControl()
        {
            InitializeComponent();
            EventManager.RegisterClassHandler(typeof(PopUpControl), ButtonBase.ClickEvent, new RoutedEventHandler(OnClick));
            
        }

        private void OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            this.IsOpen = false;
        }


        private void OnClosePopup(object sender, RoutedEventArgs routedEventArgs)
        {
            this.IsOpen = false;
        }
/*
        public static readonly RoutedEvent ClosePopupEvent = EventManager.RegisterRoutedEvent(
            "ClosePopup", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PopUpControl));

       public event RoutedEventHandler ClosePopup
       {
           add { AddHandler(ClosePopupEvent, value); }
           remove { RemoveHandler(ClosePopupEvent, value); }
       }
       */

     
    }
}

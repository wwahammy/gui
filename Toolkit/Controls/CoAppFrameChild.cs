using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CoApp.Gui.Toolkit.Controls
{
    public class CoAppFrameChild : UserControl
    {
        public CoAppFrameChild()
        {
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var newEventArgs = new RoutedEventArgs(TemplateLoadedEvent, this);
            RaiseEvent(newEventArgs);
        }

        public static readonly RoutedEvent TemplateLoadedEvent = EventManager.RegisterRoutedEvent("TemplateLoaded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CoAppFrameChild));

        // Provide CLR accessors for the event
        public event RoutedEventHandler TemplateLoaded
        {
            add { AddHandler(TemplateLoadedEvent, value); }
            remove { RemoveHandler(TemplateLoadedEvent, value); }
        }

        
    }
}

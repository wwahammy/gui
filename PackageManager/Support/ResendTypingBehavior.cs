using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using CoApp.Gui.Toolkit.Controls;
using GalaSoft.MvvmLight.Messaging;

namespace CoApp.PackageManager.Support
{
    public class ResendTypingBehavior : Behavior<Control>
    {
        protected override void OnAttached()
        {
            
            AssociatedObject.TextInput += AssociatedObjectOnTextInput;
        }

        private void AssociatedObjectOnTextInput(object sender, TextCompositionEventArgs textCompositionEventArgs)
        {

            Messenger.Default.Send(textCompositionEventArgs);
/*
            textCompositionEventArgs.Handled = true;
            var args = new TextCompositionEventArgs(textCompositionEventArgs.Device, textCompositionEventArgs.TextComposition);
            args.RoutedEvent = TypingBehavior.SendTheTextEvent;
            args.Handled = false;
            args.Source = AssociatedObject;
            
            AssociatedObject.RaiseEvent(args);*/
        }
    }
}

using System;
using System.Windows;

namespace CoApp.Gui.Toolkit.Support
{
    public class StateManager : DependencyObject
    {

        
    

        public static readonly DependencyProperty VisualStatePropertyProperty =
            DependencyProperty.RegisterAttached(
                "VisualStateProperty",
                typeof (string),
                typeof (StateManager),
                new PropertyMetadata((s, e) =>
                                         {
                                             var ctrl = s as FrameworkElement;
                                             if (ctrl == null)
                                                 throw new InvalidOperationException(
                                                     "This attached property only supports types derived from FrameworkElement.");
                                             if( e.NewValue != null)
                                             {

                                                 if (!VisualStateManager.GoToState(ctrl, (string)e.NewValue, true))
                                                 {
                                                     VisualStateManager.GoToElementState(ctrl, (string) e.NewValue, true);
                                                 }
                                             }
                                             else
                                             {
                                                 if (!VisualStateManager.GoToState(ctrl, "Normal", true))
                                                 {
                                                     VisualStateManager.GoToElementState(ctrl, "Normal", true);
                                                 }
                                             }
                                         }));



        

        public static string GetVisualStateProperty(DependencyObject obj)
        {
            return (string) obj.GetValue(VisualStatePropertyProperty);
        }

        public static void SetVisualStateProperty(DependencyObject obj, string value)
        {
            obj.SetValue(VisualStatePropertyProperty, value);
        }
    }
}
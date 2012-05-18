using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

//using Microsoft.Expression.Interactivity.Core;

namespace CoApp.PackageManager.Support
{
    public class HorizontalScrollBehavior : Behavior<ScrollViewer>
    {
        public HorizontalScrollBehavior()
        {
            // Insert code required on object creation below this point.

            //
            // The line of code below sets up the relationship between the command and the function
            // to call. Uncomment the below line and add a reference to Microsoft.Expression.Interactions
            // if you choose to use the commented out version of MyFunction and MyCommand instead of
            // creating your own implementation.
            //
            // The documentation will provide you with an example of a simple command implementation
            // you can use instead of using ActionCommand and referencing the Interactions assembly.
            //
            //this.MyCommand = new ActionCommand(this.MyFunction);
            MouseDelegate = MouseWheelMoved;
        }

       

        private MouseWheelEventHandler MouseDelegate { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            // Insert code that you would want run when the Behavior is attached to an object.
            AssociatedObject.AddHandler(UIElement.MouseWheelEvent, MouseDelegate, true);
        
           

            
        }

        private void MouseWheelMoved(object sender, MouseWheelEventArgs mouseWheelEventArgs)
        {
            if (mouseWheelEventArgs.Delta < 0)
            {
                AssociatedObject.ScrollToHorizontalOffset(AssociatedObject.HorizontalOffset - mouseWheelEventArgs.Delta);
            }

            else
            {
                AssociatedObject.ScrollToHorizontalOffset(AssociatedObject.HorizontalOffset - mouseWheelEventArgs.Delta);
            }
        }

        protected override void OnDetaching()
        {
            // Insert code that you would want run when the Behavior is removed from an object.
            AssociatedObject.RemoveHandler(UIElement.MouseWheelEvent, MouseDelegate);
            base.OnDetaching();
            
           
        }

        /*
        public ICommand MyCommand
        {
            get;
            private set;
        }
         
        private void MyFunction()
        {
            // Insert code that defines what the behavior will do when invoked.
        }
        */
    }
}
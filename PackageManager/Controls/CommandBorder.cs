using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoApp.PackageManager.Controls
{
    /// <summary>
    /// Oodles of code from our friends at Mono https://github.com/mono/moon/blob/master/class/Microsoft.SilverlightControls/Controls/Src/ButtonBase/ButtonBase.cs
    /// </summary>
    public class CommandBorder : Border
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof (ICommand), typeof (CommandBorder),
                                        new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof (object), typeof (CommandBorder),
                                        new PropertyMetadata(default(object)));

        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register("CommandTarget", typeof (IInputElement), typeof (CommandBorder),
                                        new PropertyMetadata(default(IInputElement)));

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
            "Click", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (CommandBorder));

        /// <summary> 
        /// Identifies the IsPressed dependency property. 
        /// </summary>
        public static readonly DependencyProperty IsPressedProperty =
            DependencyProperty.Register(
                "IsPressed",
                typeof (bool),
                typeof (CommandBorder),
                new PropertyMetadata(OnIsPressedPropertyChanged));

        private bool _isMouseLeftButtonDown;
        private bool _isSpaceKeyDown;

        private Point _mousePosition;

        /*
        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public IInputElement CommandTarget
        {
            get { return (IInputElement) GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }*/

        /// <summary>
        /// Gets a value that indicates whether a ButtonBase is currently 
        /// activated. 
        /// </summary>
        public bool IsPressed
        {
            get { return (bool) GetValue(IsPressedProperty); }
            protected internal set { SetValue(IsPressedProperty, value); }
        }


        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }


        private void RaiseClickEvent()
        {
            var newEventArgs = new RoutedEventArgs(ClickEvent);
            RaiseEvent(newEventArgs);
        }

        /// <summary> 
        /// IsPressedProperty property changed handler.
        /// </summary> 
        /// <param name="d">ButtonBase that changed its IsPressed.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param>
        private static void OnIsPressedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = d as CommandBorder;
            Debug.Assert(source != null,
                         "The source is not an instance of ButtonBase!");
        }

        /// <summary>
        /// Handle the MouseLeftButtonDown event.
        /// </summary> 
        /// <param name="sender">The source of the event.</param> 
        /// <param name="e">MouseButtonEventArgs.</param>
        internal void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseLeftButtonDown = true;
            OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (e.Handled)
            {
                return;
            }

            _isMouseLeftButtonDown = false;
            e.Handled = true;
            if (!_isSpaceKeyDown && IsPressed)
            {
                RaiseClickEvent();
            }

            if (!_isSpaceKeyDown)
            {
                ReleaseMouseCapture();
                IsPressed = false;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            OnKeyDownInternal(e.Key);
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            OnKeyUpInternal(e.Key);
            base.OnKeyUp(e);
        }

        private void OnKeyUpInternal(Key key)
        {
            //bool handled = false;

            // Key presses can be ignored when disabled or in ClickMode.Hover
            // or if any other key than SPACE was released. 
            if (key == Key.Space)
            {
                _isSpaceKeyDown = false;

                if (!_isMouseLeftButtonDown)
                {
                    // If the mouse isn't in use, raise the Click event if we're 
                    // in the correct click mode
                    ReleaseMouseCapture();

                    IsPressed = false;
                }
                else if (IsMouseCaptured)
                {
                    // Determine if the button should still be pressed based on
                    // the position of the mouse.
                    bool isValid = IsValidMousePosition();
                    IsPressed = isValid;
                    if (!isValid)
                    {
                        ReleaseMouseCapture();
                    }
                }

                //handled = true;
            }

            //return handled;
        }

        internal virtual bool OnKeyDownInternal(Key key)
        {
            // True if the button will handle the event, false otherwise.
            bool handled = false;

            // Key presses can be ignored when disabled or in ClickMode.Hover 

            // Hitting the SPACE key is equivalent to pressing the mouse
            // button
            if (key == Key.Space)
            {
                // Ignore the SPACE key if we already have the mouse
                // captured 
                if (!IsMouseCaptured)
                {
                    _isSpaceKeyDown = true;
                    IsPressed = true;
                    CaptureMouse();


                    RaiseClickEvent();


                    handled = true;
                }
            }
                // The ENTER key forces a click 
            else if ((key == Key.Enter) && KeyboardNavigation.GetAcceptsReturn(this))
            {
                _isSpaceKeyDown = false;
                IsPressed = false;


                RaiseClickEvent();

                handled = true;
            }
                // Any other keys pressed are irrelevant
            else if (_isSpaceKeyDown)
            {
                IsPressed = false;
                _isSpaceKeyDown = false;
                ReleaseMouseCapture();
            }


            return handled;
        }


        internal void OnMouseMove(object sender, MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        /// <summary> 
        /// Responds to the MouseMove event.
        /// </summary>
        /// <param name="e">The event data for the MouseMove event.</param> 
        private new void OnMouseMove(MouseEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            // Cache the latest mouse position.
            _mousePosition = e.GetPosition(this);


            // Determine if the button is still pressed based on the mouse's
            // current position.
            if (_isMouseLeftButtonDown &&
                IsMouseCaptured &&
                !_isSpaceKeyDown)
            {
                IsPressed = IsValidMousePosition();
            }
        }

        internal bool IsValidMousePosition()
        {
            return (_mousePosition.X >= 0.0) &&
                   (_mousePosition.X <= ActualWidth) &&
                   (_mousePosition.Y >= 0.0) &&
                   (_mousePosition.Y <= ActualHeight);
        }
    }
}
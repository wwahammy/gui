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

namespace CoApp.PackageManager.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CoApp.PackageManager.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CoApp.PackageManager.Controls;assembly=CoApp.PackageManager.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ToggleDropDown/>
    ///
    /// </summary>
    public class ToggleDropDown : Control
    {
        private Popup _popup;
        static ToggleDropDown()
        {
            EventManager.RegisterClassHandler(typeof(ToggleDropDown), ButtonBase.ClickEvent,
                                              new RoutedEventHandler(HandleChildClick), true);
        }

        private static void HandleChildClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var toggle = sender as ToggleDropDown;
            if (toggle == null)
                return;

            var button = routedEventArgs.OriginalSource as Button;
            if (button == null)
                return;

            var value = button.GetValue(CloseContainingDropDownProperty);
            if (value != null)
            {
                var val = (bool)value;
                if (val)
                {
                    if (toggle._popup != null)
                        toggle._popup.IsOpen = false;
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _popup = GetTemplateChild("Popup") as Popup;
            if (_popup != null)
                _popup.CustomPopupPlacementCallback = CustomPopUpTarget;
        }

        public static readonly DependencyProperty PopupContentProperty =
            DependencyProperty.Register("PopupContent", typeof(object), typeof(ToggleDropDown), new PropertyMetadata(default(object)));

        public object PopupContent
        {
            get { return (object)GetValue(PopupContentProperty); }
            set { SetValue(PopupContentProperty, value); }
        }

        public static readonly DependencyProperty CloseContainingDropDownProperty =
            DependencyProperty.RegisterAttached("CloseContainingDropDown", typeof(bool), typeof(ToggleDropDown), new PropertyMetadata(default(bool)));


        public static void SetCloseContainingDropDown(Button element, bool value)
        {
            element.SetValue(CloseContainingDropDownProperty, value);
        }

        public static bool GetCloseContainingDropDown(Button element)
        {
            return (bool)element.GetValue(CloseContainingDropDownProperty);
        }


        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(ToggleDropDown), new PropertyMetadata(default(bool)));

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public static readonly DependencyProperty PopupAlignmentProperty =
            DependencyProperty.Register("PopupAlignment", typeof(HorizontalAlignment), typeof(ToggleDropDown), new PropertyMetadata(default(HorizontalAlignment)));

        public HorizontalAlignment PopupAlignment
        {
            get { return (HorizontalAlignment)GetValue(PopupAlignmentProperty); }
            set { SetValue(PopupAlignmentProperty, value); }
        }

        private CustomPopupPlacement[] CustomPopUpTarget(Size popupSize, Size targetSize, Point offset)
        {
            var height = targetSize.Height + PopUpOffset.Y;

            var x = targetSize.Width - popupSize.Width + PopUpOffset.X;


            var placement = new CustomPopupPlacement(new Point(x, height), PopupPrimaryAxis.Horizontal);
            return new[] { placement };
        }

        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register("ButtonContent", typeof(object), typeof(ToggleDropDown), new PropertyMetadata(default(object)));

        public object ButtonContent
        {
            get { return (object)GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }

        public static readonly DependencyProperty ToggleButtonStyleProperty =
            DependencyProperty.Register("ToggleButtonStyle", typeof(Style), typeof(ToggleDropDown), new PropertyMetadata(default(Style)));

        public Style ToggleButtonStyle
        {
            get { return (Style)GetValue(ToggleButtonStyleProperty); }
            set { SetValue(ToggleButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty PopUpOffsetProperty =
            DependencyProperty.Register("PopUpOffset", typeof(Point), typeof(ToggleDropDown), new PropertyMetadata(default(Point)));

        public Point PopUpOffset
        {
            get { return (Point)GetValue(PopUpOffsetProperty); }
            set { SetValue(PopUpOffsetProperty, value); }
        }


        public static readonly DependencyProperty PopupTemplateProperty =
            DependencyProperty.Register("PopupTemplate", typeof (DataTemplate), typeof (ToggleDropDown), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate PopupTemplate
        {
            get { return (DataTemplate) GetValue(PopupTemplateProperty); }
            set { SetValue(PopupTemplateProperty, value); }
        }
  
    }
}

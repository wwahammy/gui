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
using CoApp.Gui.Toolkit.Controls;
using CoApp.Gui.Toolkit.Support;

namespace CoApp.PackageManager.Controls
{
    /// <summary>
    /// Interaction logic for ToggleDropDown.xaml
    /// </summary>
    public partial class ToggleDropDown
    {
      

        static ToggleDropDown()
        {
            EventManager.RegisterClassHandler(typeof (ToggleDropDown), ButtonBase.ClickEvent,
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
                var val = (bool) value;
                if (val)
                {
                    toggle.Popup.IsOpen = false;
                }
            }
        }

        public ToggleDropDown()
        {
            InitializeComponent();
            Popup.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(CustomPopUpTarget);

        }

        public static readonly DependencyProperty PopupContentProperty =
            DependencyProperty.Register("PopupContent", typeof(object), typeof(ToggleDropDown), new PropertyMetadata(default(object)));

        public object PopupContent
        {
            get { return (object)GetValue(PopupContentProperty); }
            set { SetValue(PopupContentProperty, value); }
        }

        public static readonly DependencyProperty CloseContainingDropDownProperty =
            DependencyProperty.RegisterAttached("CloseContainingDropDown", typeof (bool), typeof (ToggleDropDown), new PropertyMetadata(default(bool)));
       

        public static void SetCloseContainingDropDown(Button element, bool value)
        {
            element.SetValue(CloseContainingDropDownProperty, value);
        }

        public static bool GetCloseContainingDropDown(Button element)
        {
            return (bool) element.GetValue(CloseContainingDropDownProperty);
        }


        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof (bool), typeof (ToggleDropDown), new PropertyMetadata(default(bool)));

        public bool IsChecked
        {
            get { return (bool) GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public static readonly DependencyProperty PopupAlignmentProperty =
            DependencyProperty.Register("PopupAlignment", typeof (HorizontalAlignment), typeof (ToggleDropDown), new PropertyMetadata(default(HorizontalAlignment)));

        public HorizontalAlignment PopupAlignment
        {
            get { return (HorizontalAlignment) GetValue(PopupAlignmentProperty); }
            set { SetValue(PopupAlignmentProperty, value); }
        }

        private CustomPopupPlacement[] CustomPopUpTarget(Size popupSize, Size targetSize, Point offset)
        {
            var height = targetSize.Height;
            var x = targetSize.Width - popupSize.Width;

            var placement = new CustomPopupPlacement(new Point(x, height), PopupPrimaryAxis.Horizontal);
            return new[] {placement};
        }

        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register("ButtonContent", typeof (object), typeof (ToggleDropDown), new PropertyMetadata(default(object)));

        public object ButtonContent
        {
            get { return (object) GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }

        public static readonly DependencyProperty ToggleButtonStyleProperty =
            DependencyProperty.Register("ToggleButtonStyle", typeof (Style), typeof (ToggleDropDown), new PropertyMetadata(default(Style)));

        public Style ToggleButtonStyle
        {
            get { return (Style) GetValue(ToggleButtonStyleProperty); }
            set { SetValue(ToggleButtonStyleProperty, value); }
        }
  
    }
}

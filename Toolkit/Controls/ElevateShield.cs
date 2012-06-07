using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CoApp.Toolkit.Win32;

namespace CoApp.Gui.Toolkit.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CoApp.Gui.Toolkit.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CoApp.Gui.Toolkit.Controls;assembly=CoApp.Gui.Toolkit.Controls"
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
    ///    
    ///
    /// </summary>
    public class ElevateShield : Control
    {


        static ElevateShield()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ElevateShield), new FrameworkPropertyMetadata(typeof(ElevateShield)));
        }

        public static readonly DependencyProperty IsWindowsVistaOrEarlierProperty =
            DependencyProperty.Register("IsWindowsVistaOrEarlier", typeof(bool), typeof(ElevateShield), new PropertyMetadata(WindowsVersionInfo.IsVistaOrPrior));

        public bool IsWindowsVistaOrEarlier
        {
            get { return (bool) GetValue(IsWindowsVistaOrEarlierProperty); }
            set { SetValue(IsWindowsVistaOrEarlierProperty, value); }
        }

        public static readonly DependencyProperty ShieldSourceProperty =
            DependencyProperty.Register("ShieldSource", typeof (BitmapSource), typeof (ElevateShield), new PropertyMetadata(default(BitmapSource)));

        public BitmapSource ShieldSource
        {
            get { return (BitmapSource) GetValue(ShieldSourceProperty); }
            set { SetValue(ShieldSourceProperty, value); }
        }
       

        public static readonly DependencyProperty OldStyleUACProperty =
            DependencyProperty.Register("OldStyleUAC", typeof (BitmapSource), typeof (ElevateShield), new PropertyMetadata(CreateBitmapSource()));

        public BitmapSource OldStyleUAC
        {
            get { return (BitmapSource) GetValue(OldStyleUACProperty); }
            set { SetValue(OldStyleUACProperty, value); }
        }

        private static BitmapSource CreateBitmapSource()
        {
            var shield = SystemIcons.Shield;
            return Imaging.CreateBitmapSourceFromHIcon(shield.Handle, Int32Rect.Empty,
                                                                       BitmapSizeOptions.FromEmptyOptions());
        }
    }
}

using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CoApp.Toolkit.Win32;

namespace CoApp.Gui.Toolkit.Controls
{
    /// <summary>
    /// Interaction logic for ElevateShield.xaml
    /// </summary>
    public partial class ElevateShield : UserControl
    {
        public ElevateShield()
        {
            if (WindowsVersionInfo.IsVistaOrPrior)
            {
                var shield = SystemIcons.Shield;
                ShieldSource = Imaging.CreateBitmapSourceFromHIcon(shield.Handle, Int32Rect.Empty,
                                                                       BitmapSizeOptions.FromEmptyOptions());
            }
            else
            {
                //we have to get the right one for Win7
                ShieldSource =
                    new BitmapImage(new Uri("pack://application:,,,/CoApp.Gui.Toolkit;component/Resources/UAC-Win7.png"));
            }


            InitializeComponent();
        }

        public static readonly DependencyProperty ShieldSourceProperty =
            DependencyProperty.Register("ShieldSource", typeof (ImageSource), typeof (ElevateShield), new PropertyMetadata(default(ImageSource)));

        public ImageSource ShieldSource
        {
            get { return (ImageSource) GetValue(ShieldSourceProperty); }
            set { SetValue(ShieldSourceProperty, value); }
        }
        
    }
}
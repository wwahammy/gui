using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using CoApp.Toolkit.Win32;

namespace CoApp.Updater.Controls
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
                ShieldIconSource = Imaging.CreateBitmapSourceFromHIcon(shield.Handle, Int32Rect.Empty,
                                                                       BitmapSizeOptions.FromEmptyOptions());
            }
            else
            {
                //we have to get the right one for Win7
                ShieldIconSource =
                    new BitmapImage(new Uri("pack://application:,,,/CoApp.Updater;component/Resources/UAC-Win7.png"));
            }


            InitializeComponent();
        }

        public BitmapSource ShieldIconSource { get; set; }
    }
}
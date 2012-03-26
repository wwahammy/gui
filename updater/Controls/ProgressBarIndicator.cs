using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CoApp.Updater.Controls
{
    public class ProgressBarIndicator : ProgressBar
    {
        public ProgressBarIndicator()
        {
            SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            RealWidth = sizeChangedEventArgs.NewSize.Width;
        }

        public static readonly DependencyProperty RealWidthProperty =
            DependencyProperty.Register("RealWidth", typeof (double), typeof (ProgressBarIndicator), new PropertyMetadata(default(double)));

        public double RealWidth
        {
            get { return (double) GetValue(RealWidthProperty); }
            set { SetValue(RealWidthProperty, value); }
        }

        
    }
}

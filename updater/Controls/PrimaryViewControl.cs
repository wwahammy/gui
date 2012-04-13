using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoApp.Updater.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CoApp.Updater.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CoApp.Updater.Controls;assembly=CoApp.Updater.Controls"
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
    ///     <MyNamespace:PrimaryViewControl/>
    ///
    /// </summary>
    public class PrimaryViewControl : ContentControl
    {
        static PrimaryViewControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PrimaryViewControl), new FrameworkPropertyMetadata(typeof(PrimaryViewControl)));
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(PrimaryViewControl), new PropertyMetadata(default(ImageSource)));

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty TopGradientColorProperty =
            DependencyProperty.Register("TopGradientColor", typeof(Color), typeof(PrimaryViewControl), new PropertyMetadata(default(Color)));

        public Color TopGradientColor
        {
            get { return (Color)GetValue(TopGradientColorProperty); }
            set { SetValue(TopGradientColorProperty, value); }
        }

        public static readonly DependencyProperty BottomGradientColorProperty =
            DependencyProperty.Register("BottomGradientColor", typeof(Color), typeof(PrimaryViewControl), new PropertyMetadata(default(Color)));

        public Color BottomGradientColor
        {
            get { return (Color)GetValue(BottomGradientColorProperty); }
            set { SetValue(BottomGradientColorProperty, value); }
        }


        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(PrimaryViewControl), new PropertyMetadata(default(string)));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
    }
}

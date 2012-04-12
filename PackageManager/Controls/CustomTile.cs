using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    ///     <MyNamespace:CustomTile/>
    ///
    /// </summary>
    public class CustomTile : Control
    {
        static CustomTile()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomTile), new FrameworkPropertyMetadata(typeof(CustomTile)));
        }

        public static readonly DependencyProperty PrimaryColorBrushProperty =
            DependencyProperty.Register("PrimaryColorBrush", typeof (SolidColorBrush), typeof (CustomTile), new PropertyMetadata(default(SolidColorBrush)));

        public SolidColorBrush PrimaryColorBrush
        {
            get { return (SolidColorBrush) GetValue(PrimaryColorBrushProperty); }
            set { SetValue(PrimaryColorBrushProperty, value); }
        }


        public static readonly DependencyProperty IconSourceProperty =
            DependencyProperty.Register("IconSource", typeof(BitmapSource), typeof(CustomTile), new PropertyMetadata(default(BitmapSource)));

        public BitmapSource IconSource
        {
            get { return (BitmapSource)GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof (string), typeof (CustomTile), new PropertyMetadata(default(string)));

        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register("Rating", typeof (double?), typeof (CustomTile), new PropertyMetadata(default(double?)));


        /// <summary>
        /// null if no ratings have ever been set on this
        /// </summary>
        public double? Rating
        {
            get { return (double?) GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }


        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof (string), typeof (CustomTile), new PropertyMetadata(default(string)));

        public string Description
        {
            get { return (string) GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }



        public static readonly DependencyProperty SummaryProperty =
            DependencyProperty.Register("Summary", typeof (string), typeof (CustomTile), new PropertyMetadata(default(string)));

        public string Summary
        {
            get { return (string) GetValue(SummaryProperty); }
            set { SetValue(SummaryProperty, value); }
        }

    }
}

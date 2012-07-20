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
using CoApp.PackageManager.ViewModel;

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
    ///
    /// </summary>
    public class ActivityNotification : Control
    {
        static ActivityNotification()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ActivityNotification), new FrameworkPropertyMetadata(typeof(ActivityNotification)));
        }

        public static readonly DependencyProperty ButtonCommandProperty =
            DependencyProperty.Register("ButtonCommand", typeof (ICommand), typeof (ActivityNotification), new PropertyMetadata(default(ICommand)));

        public ICommand ButtonCommand
        {
            get { return (ICommand) GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        public static readonly DependencyProperty NumberOfActivitiesProperty =
            DependencyProperty.Register("NumberOfActivities", typeof (int), typeof (ActivityNotification), new PropertyMetadata(default(int), NumberChanged));

        private static void NumberChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var notif = dependencyObject as ActivityNotification;
            if (notif != null)
            {

                if (notif.NumberOfActivities == 0 && notif.NumberOfActivityFailures == 0 && notif.NumberofActivitiesFinished == 0)
                {
                    notif.ShouldShow = false;
                }
                else
                {
                    notif.ShouldShow = true;
                    var sb = new StringBuilder();
                    if (notif.NumberOfActivityFailures > 0)
                        sb.AppendFormat("{0} failed, ", notif.NumberOfActivityFailures);
                    if (notif.NumberOfActivities > 0)
                        sb.AppendFormat("{0} in progress, ", notif.NumberOfActivities);
                    if (notif.NumberofActivitiesFinished > 0)
                        sb.AppendFormat("{0} finished, ", notif.NumberofActivitiesFinished);

                    sb.Remove(sb.Length - 2, 2);
                    notif.Text = sb.ToString();
                }
                //recreateText
                //handle should show
            }
        }

        public int NumberOfActivities
        {
            get { return (int) GetValue(NumberOfActivitiesProperty); }
            set { SetValue(NumberOfActivitiesProperty, value); }
        }

        public static readonly DependencyProperty NumberOfActivityFailuresProperty =
            DependencyProperty.Register("NumberOfActivityFailures", typeof(int), typeof(ActivityNotification), new PropertyMetadata(default(int), NumberChanged));

        public int NumberOfActivityFailures
        {
            get { return (int) GetValue(NumberOfActivityFailuresProperty); }
            set { SetValue(NumberOfActivityFailuresProperty, value); }
        }

        public static readonly DependencyProperty NumberofActivitiesFinishedProperty =
            DependencyProperty.Register("NumberofActivitiesFinished", typeof(int), typeof(ActivityNotification), new PropertyMetadata(default(int), NumberChanged));

        public int NumberofActivitiesFinished
        {
            get { return (int) GetValue(NumberofActivitiesFinishedProperty); }
            set { SetValue(NumberofActivitiesFinishedProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof (string), typeof (ActivityNotification), new PropertyMetadata(default(string)));

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty ShouldShowProperty =
            DependencyProperty.Register("ShouldShow", typeof (bool), typeof (ActivityNotification), new PropertyMetadata(default(bool)));

        public bool ShouldShow
        {
            get { return (bool) GetValue(ShouldShowProperty); }
            set { SetValue(ShouldShowProperty, value); }
        }
    }
}

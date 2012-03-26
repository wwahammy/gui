using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
    ///     <MyNamespace:UnblockControl/>
    ///
    /// </summary>
    public class UnblockControl : Control
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof (string), typeof (UnblockControl),
                                        new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ElevatedCommandProperty =
            DependencyProperty.Register("ElevatedCommand", typeof (ICommand), typeof (UnblockControl),
                                        new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty UnelevatedCommandProperty =
            DependencyProperty.Register("UnelevatedCommand", typeof (ICommand), typeof (UnblockControl),
                                        new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty MustElevateProperty =
            DependencyProperty.Register("MustElevate", typeof (bool), typeof (UnblockControl),
                                        new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty InErrorProperty =
            DependencyProperty.Register("InError", typeof (bool?), typeof (UnblockControl),
                                        new PropertyMetadata(default(bool?)));


        public static readonly DependencyProperty ShowErrorProperty =
            DependencyProperty.Register("ShowError", typeof (ICommand), typeof (UnblockControl),
                                        new PropertyMetadata(default(ICommand)));


        public static readonly DependencyProperty WaitingOnUnblockProperty =
            DependencyProperty.Register("WaitingOnUnblock", typeof (bool), typeof (UnblockControl),
                                        new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty ElevatedCommandParameterProperty =
            DependencyProperty.Register("ElevatedCommandParameter", typeof (object), typeof (UnblockControl),
                                        new PropertyMetadata(default(object)));

        public static readonly DependencyProperty UnelevatedCommandParameterProperty =
            DependencyProperty.Register("UnelevatedCommandParameter", typeof (object), typeof (UnblockControl),
                                        new PropertyMetadata(default(object)));


        static UnblockControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (UnblockControl),
                                                     new FrameworkPropertyMetadata(typeof (UnblockControl)));
        }

        public object ElevatedCommandParameter
        {
            get { return GetValue(ElevatedCommandParameterProperty); }
            set { SetValue(ElevatedCommandParameterProperty, value); }
        }

        public object UnelevatedCommandParameter
        {
            get { return GetValue(UnelevatedCommandParameterProperty); }
            set { SetValue(UnelevatedCommandParameterProperty, value); }
        }

        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public ICommand ElevatedCommand
        {
            get { return (ICommand) GetValue(ElevatedCommandProperty); }
            set { SetValue(ElevatedCommandProperty, value); }
        }

        public ICommand UnelevatedCommand
        {
            get { return (ICommand) GetValue(UnelevatedCommandProperty); }
            set { SetValue(UnelevatedCommandProperty, value); }
        }

        public bool MustElevate
        {
            get { return (bool) GetValue(MustElevateProperty); }
            set { SetValue(MustElevateProperty, value); }
        }

        public bool? InError
        {
            get { return (bool?) GetValue(InErrorProperty); }
            set { SetValue(InErrorProperty, value); }
        }

        public ICommand ShowError
        {
            get { return (ICommand) GetValue(ShowErrorProperty); }
            set { SetValue(ShowErrorProperty, value); }
        }

        public bool WaitingOnUnblock
        {
            get { return (bool) GetValue(WaitingOnUnblockProperty); }
            set { SetValue(WaitingOnUnblockProperty, value); }
        }
    }
}
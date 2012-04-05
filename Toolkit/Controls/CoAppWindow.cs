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
    ///     <MyNamespace:CoAppWindow/>
    ///
    /// </summary>
    [TemplatePart(Name = MaxRestoreName, Type = typeof(Button))]
    [TemplatePart(Name = MinimizeName, Type = typeof(Button))]
    [TemplatePart(Name = NavFrameName, Type = typeof(CoAppFrame))]
    [TemplatePart(Name = HeaderName, Type= typeof(Grid))]
    public class CoAppWindow : Window
    {
        private const string NavFrameName = "NavFrame";
        private const string MaxRestoreName = "MaxRestore";
        private const string MinimizeName = "Minimize";
        private const string HeaderName = "Header";
        private CoAppFrame _navFrame;
        private Button _maxRestore, _minimize;
        private Grid _header;

        static CoAppWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CoAppWindow), new FrameworkPropertyMetadata(typeof(CoAppWindow)));
        }
        private void HeaderDragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton != MouseButtonState.Pressed && e.MiddleButton != MouseButtonState.Pressed)
                DragMove();
        }

        private void MinimizeClickWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaxRestoreClickWindow(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                _maxRestore.Content = "1";
                WindowState = WindowState.Normal;
            }
            else
            {
                _maxRestore.Content = "2";
                WindowState = WindowState.Maximized;
            }
        }

        public static readonly DependencyProperty CoAppFrameResourcesProperty =
            DependencyProperty.Register("CoAppFrameResources", typeof(ResourceDictionary), typeof(CoAppWindow), new PropertyMetadata(default(ResourceDictionary), CoAppResourceDictionaryChanged));

        private static void CoAppResourceDictionaryChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var win = (CoAppWindow)dependencyObject;
            if (dependencyPropertyChangedEventArgs.OldValue != null && dependencyPropertyChangedEventArgs.OldValue is ResourceDictionary)// && win._navFrame != null)
            {
                win.Resources.MergedDictionaries.Remove(
                    (ResourceDictionary)dependencyPropertyChangedEventArgs.OldValue);
            }

            if (dependencyPropertyChangedEventArgs.NewValue != null && dependencyPropertyChangedEventArgs.NewValue is ResourceDictionary)// && win._navFrame != null)
            {
                win.Resources.MergedDictionaries.Add(
                    (ResourceDictionary)dependencyPropertyChangedEventArgs.NewValue);
            }
        }


        public ResourceDictionary CoAppFrameResources
        {
            get { return (ResourceDictionary)GetValue(CoAppFrameResourcesProperty); }
            set { SetValue(CoAppFrameResourcesProperty, value); }
        }

        public static readonly DependencyProperty FrameContentProperty =
            DependencyProperty.Register("FrameContent", typeof(object), typeof(CoAppWindow), new PropertyMetadata(default(object)));

        public object FrameContent
        {
            get { return (object)GetValue(FrameContentProperty); }
            set { SetValue(FrameContentProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

           

            if (_maxRestore != null)
            {
                _maxRestore.Click -= MaxRestoreClickWindow;
            }

            if (_minimize != null)
            {
                _minimize.Click -= MinimizeClickWindow;
            }

            if (_header != null)
            {
                _header.MouseLeftButtonDown -= HeaderDragWindow;
            }

            _navFrame = GetTemplateChild(NavFrameName) as CoAppFrame;
            _maxRestore = GetTemplateChild(MaxRestoreName) as Button;
            _minimize = GetTemplateChild(MinimizeName) as Button;
            _header = GetTemplateChild(HeaderName) as Grid;

            if (_maxRestore != null)
            {
                _maxRestore.Click += MaxRestoreClickWindow;
            }

            if (_minimize != null)
            {
                _minimize.Click += MinimizeClickWindow;
            }

            if (_header != null)
            {
                _header.MouseLeftButtonDown += HeaderDragWindow;
            }


        }

    }
}

using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using CoApp.Gui.Toolkit.Support;

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
    [TemplatePart(Name = MAX_RESTORE_NAME, Type = typeof (Button))]
    [TemplatePart(Name = MINIMIZE_NAME, Type = typeof (Button))]
    [TemplatePart(Name = NAV_FRAME_NAME, Type = typeof (CoAppFrame))]
    [TemplatePart(Name = HEADER_NAME, Type = typeof (Grid))]
    [TemplateVisualState(Name = "Base", GroupName = "MainStates")]
    [TemplateVisualState(Name = "Showing", GroupName = "MainStates")]
    [TemplateVisualState(Name = "Loading", GroupName = "MainStates")]
    public class CoAppWindow : Window
    {
        private const string NAV_FRAME_NAME = "NavFrame";
        private const string MAX_RESTORE_NAME = "MaxRestore";
        private const string MINIMIZE_NAME = "Minimize";
        private const string HEADER_NAME = "Header";
        

        
    

        public static readonly DependencyProperty FrameResourcesProperty =
            DependencyProperty.Register("FrameResources", typeof (ResourceDictionary), typeof (CoAppWindow),
                                        new PropertyMetadata(default(ResourceDictionary), FrameResourceCallback));

        public static readonly DependencyProperty CoAppFrameResourcesProperty =
            DependencyProperty.Register("CoAppFrameResources", typeof (ResourceDictionary), typeof (CoAppWindow),
                                        new PropertyMetadata(default(ResourceDictionary), CoAppResourceDictionaryChanged));

        public static readonly DependencyProperty AdditionalHeaderItemsProperty =
            DependencyProperty.Register("AdditionalHeaderItems", typeof (IEnumerable), typeof (CoAppWindow),
                                        new PropertyMetadata(default(IEnumerable)));


        public static readonly DependencyProperty MoreHeaderItemsProperty =
            DependencyProperty.RegisterAttached("MoreHeaderItems", typeof (IEnumerable), typeof (CoAppWindow), new PropertyMetadata(default(IEnumerable)));



        private static void MoreHeaderItemsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var win = UIHelper.FindVisualParent<CoAppWindow>(dependencyObject);

            if (win != null)
            {
                win.AdditionalHeaderItems = (IEnumerable) dependencyObject.GetValue(MoreHeaderItemsProperty);
            }
        }

        public static void SetMoreHeaderItems(UIElement element, IEnumerable value)
        {
            element.SetValue(MoreHeaderItemsProperty, value);
        }

        public static IEnumerable GetMoreHeaderItems(UIElement element)
        {
            return (IEnumerable) element.GetValue(MoreHeaderItemsProperty);
        }

        private Grid _header;

        private Button _maxRestore, _minimize;
        private CoAppFrame _navFrame;

        static CoAppWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (CoAppWindow),
                                                     new FrameworkPropertyMetadata(typeof (CoAppWindow)));

            EventManager.RegisterClassHandler(typeof(CoAppWindow), CoAppFrameChild.TemplateLoadedEvent, new RoutedEventHandler(TemplateLoadedHandle));
        }

        internal static void TemplateLoadedHandle(object sender, RoutedEventArgs a)
        {
            var original = (CoAppFrameChild) a.OriginalSource;
            var win = (CoAppWindow) sender;

            BindingOperations.SetBinding(win, AdditionalHeaderItemsProperty,
                                         new Binding
                                             {Source = original, Path = new PropertyPath(MoreHeaderItemsProperty)});

        }

        public IEnumerable AdditionalHeaderItems
        {
            get { return (IEnumerable) GetValue(AdditionalHeaderItemsProperty); }
            set { SetValue(AdditionalHeaderItemsProperty, value); }
        }

        public ResourceDictionary FrameResources
        {
            get { return (ResourceDictionary) GetValue(FrameResourcesProperty); }
            set { SetValue(FrameResourcesProperty, value); }
        }

        public ResourceDictionary CoAppFrameResources
        {
            get { return (ResourceDictionary) GetValue(CoAppFrameResourcesProperty); }
            set { SetValue(CoAppFrameResourcesProperty, value); }
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

        private static void FrameResourceCallback(DependencyObject dependencyObject,
                                                  DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var win = (CoAppWindow) dependencyObject;
            var fe = (FrameworkElement) win.FindName("NavFrame");
            if (fe != null) fe.Resources = (ResourceDictionary) dependencyPropertyChangedEventArgs.NewValue;
        }


        private static void CoAppResourceDictionaryChanged(DependencyObject dependencyObject,
                                                           DependencyPropertyChangedEventArgs
                                                               dependencyPropertyChangedEventArgs)
        {
            var win = (CoAppWindow) dependencyObject;
            if (dependencyPropertyChangedEventArgs.OldValue != null &&
                dependencyPropertyChangedEventArgs.OldValue is ResourceDictionary) // && win._navFrame != null)
            {
                win.Resources.MergedDictionaries.Remove(
                    (ResourceDictionary) dependencyPropertyChangedEventArgs.OldValue);
            }

            if (dependencyPropertyChangedEventArgs.NewValue != null &&
                dependencyPropertyChangedEventArgs.NewValue is ResourceDictionary) // && win._navFrame != null)
            {
                win.Resources.MergedDictionaries.Add(
                    (ResourceDictionary) dependencyPropertyChangedEventArgs.NewValue);
            }
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

            _navFrame = GetTemplateChild(NAV_FRAME_NAME) as CoAppFrame;
            _maxRestore = GetTemplateChild(MAX_RESTORE_NAME) as Button;
            _minimize = GetTemplateChild(MINIMIZE_NAME) as Button;
            _header = GetTemplateChild(HEADER_NAME) as Grid;

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
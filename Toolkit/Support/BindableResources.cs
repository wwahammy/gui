using System.Windows;

namespace CoApp.Gui.Toolkit.Support
{
    public static class BindableResources
    {
        public static readonly DependencyProperty ResourcesProperty =
            DependencyProperty.RegisterAttached("Resources", typeof (ResourceDictionary), typeof (BindableResources),
                                                new PropertyMetadata(default(ResourceDictionary),
                                                                     PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject,
                                                    DependencyPropertyChangedEventArgs
                                                        dependencyPropertyChangedEventArgs)
        {
            var fe = (FrameworkElement) dependencyObject;
            if (fe == null)
                return;

            if (dependencyPropertyChangedEventArgs.OldValue != null &&
                dependencyPropertyChangedEventArgs.OldValue is ResourceDictionary)
            {
                RemoveDictionaryFromElement(fe, (ResourceDictionary) dependencyPropertyChangedEventArgs.OldValue);
            }

            if (dependencyPropertyChangedEventArgs.NewValue != null &&
                dependencyPropertyChangedEventArgs.NewValue is ResourceDictionary)
            {
                AddDictionaryFromElement(fe, (ResourceDictionary) dependencyPropertyChangedEventArgs.NewValue);
            }
        }

        public static void SetResources(FrameworkElement element, ResourceDictionary value)
        {
            element.SetValue(ResourcesProperty, value);
        }

        public static ResourceDictionary GetResources(FrameworkElement element)
        {
            return (ResourceDictionary) element.GetValue(ResourcesProperty);
        }

        private static void RemoveDictionaryFromElement(FrameworkElement fe, ResourceDictionary rd)
        {
            if (fe.Resources != null && fe.Resources.MergedDictionaries.Contains(rd))
            {
                fe.Resources.MergedDictionaries.Remove(rd);
            }
        }

        private static void AddDictionaryFromElement(FrameworkElement fe, ResourceDictionary rd)
        {
            if (fe.Resources == null)
            {
                fe.Resources = new ResourceDictionary();
            }

            fe.Resources.MergedDictionaries.Add(rd);
        }
    }
}
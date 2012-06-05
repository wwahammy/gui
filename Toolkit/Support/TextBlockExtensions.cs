using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CoApp.Gui.Toolkit.Support
{
    /// <summary>
    /// 
    /// </summary>
    /// <from>http://www.eugenedotnet.com/2011/04/binding-text-containing-tags-to-textblock-inlines-using-attached-property-in-silverlight-for-windows-phone-7/</from>
    public static class TextBlockExtensions
    {
        public static readonly DependencyProperty InlinesListProperty =
            DependencyProperty.RegisterAttached("InlinesList", typeof (IEnumerable<Inline>), typeof (TextBlockExtensions), new PropertyMetadata(default(IEnumerable<Inline>), InlinesChanged));

        private static void InlinesChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var text = dependencyObject as TextBlock;
            var enumerable = dependencyPropertyChangedEventArgs.NewValue as IEnumerable<Inline>;
            if (text != null && enumerable != null)
            {
                text.Inlines.Clear();

                foreach (var i in enumerable)
                {
                    text.Inlines.Add(i);
                }
            }
        }

        public static void SetInlinesList(TextBlock element, IEnumerable<Inline> value)
        {
            element.SetValue(InlinesListProperty, value);
        }

        public static IEnumerable<Inline> GetInlinesList(TextBlock element)
        {
            return (IEnumerable<Inline>) element.GetValue(InlinesListProperty);
        }
    }
}

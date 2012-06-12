using System;
using System.Collections.Generic;
using System.Linq;
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
            DependencyProperty.RegisterAttached("InlinesList", typeof (IEnumerable<Inline>), typeof (TextBlockExtensions), new PropertyMetadata(Enumerable.Empty<Inline>(), InlinesChanged));

        private static void InlinesChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var text = dependencyObject as TextBlock;
            var enumerable = dependencyPropertyChangedEventArgs.NewValue as IEnumerable<Inline>;
            if (text != null && enumerable != null)
            {
               RecreateInlines(text, enumerable);
            }
        }

        private static void RecreateInlines(TextBlock t, Inline prefix)
        {
            RecreateInlines(t, prefix, (IEnumerable<Inline>)t.GetValue(InlinesListProperty));
        }

        private static void RecreateInlines(TextBlock t, IEnumerable<Inline> list)
        {
            RecreateInlines(t, (Inline) t.GetValue(PrefixInlineListProperty), list);
        }

        private static void RecreateInlines(TextBlock t, Inline prefix, IEnumerable<Inline> list)
        {
            t.Inlines.Clear();

            if (prefix != null)
                t.Inlines.Add(prefix);
            foreach (var i in list)
            {
                t.Inlines.Add(i);
            }

            t.InvalidateMeasure();
        }

        public static void SetInlinesList(TextBlock element, IEnumerable<Inline> value)
        {
            element.SetValue(InlinesListProperty, value);
        }

        public static IEnumerable<Inline> GetInlinesList(TextBlock element)
        {
            return (IEnumerable<Inline>) element.GetValue(InlinesListProperty);
        }


        public static readonly DependencyProperty PrefixInlineListProperty =
            DependencyProperty.RegisterAttached("PrefixInlineList", typeof (Inline), typeof (TextBlockExtensions), new PropertyMetadata(default(Inline), PrefixChanged));

        private static void PrefixChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var text = dependencyObject as TextBlock;
            var inline = dependencyPropertyChangedEventArgs.NewValue as Inline;
            if (text != null && inline != null)
            {
               RecreateInlines(text, inline);
            }
        }

        public static void SetPrefixInlineList(UIElement element, Inline value)
        {
            element.SetValue(PrefixInlineListProperty, value);
        }

        public static Inline GetPrefixInlineList(UIElement element)
        {
            return (Inline) element.GetValue(PrefixInlineListProperty);
        }
    }
}

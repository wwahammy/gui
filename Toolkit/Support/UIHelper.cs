using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace CoApp.Gui.Toolkit.Support
{
    public static class UIHelper
    {
        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">A direct or indirect child of the queried item.</param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, a null reference is being returned.</returns>
        /// <from>http://stackoverflow.com/questions/636383/wpf-ways-to-find-controls</from>
        public static T FindVisualParent<T>(this DependencyObject child)
          where T : DependencyObject
        {
            // get parent item
            var parentObject = VisualTreeHelper.GetParent(child);

            // we’ve reached the end of the tree
            if (parentObject == null) return null;

            // check if the parent matches the type we’re looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                // use recursion to proceed with next level
                return FindVisualParent<T>(parentObject);
            }
        }

        public static IEnumerable<TChildItem> FindVisualChildren<TChildItem>(this DependencyObject obj)
            where TChildItem : DependencyObject
        {
            if (obj == null)
                yield break;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is TChildItem)
                    yield return (TChildItem)child;
                else
                {
                    var childOfChild = FindVisualChildren<TChildItem>(child);
                    if (childOfChild != null && childOfChild.Any())
                    {
                       foreach (var c in childOfChild)
                       {
                           yield return c;
                       }
                    }
                }
            }
            yield break;
        }

        public static IEnumerable<Visual> GetDirectVisualChildren(this DependencyObject obj)
        {
            if (obj == null)
                yield break;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is Visual)
                    yield return (Visual)child;
            }
        }
    }
}

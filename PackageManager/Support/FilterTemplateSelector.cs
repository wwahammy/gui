using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CoApp.PackageManager.Support
{
    /// <summary>
    /// 
    /// </summary>
    /// <from>http://stackoverflow.com/questions/3468866/tabcontrol-with-add-new-tab-button</from>
    public class FilterTemplateSelector : DataTemplateSelector
    {

        public DataTemplate ItemTemplate { get; set; }
        public DataTemplate NewItemTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == CollectionView.NewItemPlaceholder)
            {
                return NewItemTemplate;
            }
            else
            {
                return ItemTemplate;
            }
        }
    }
}

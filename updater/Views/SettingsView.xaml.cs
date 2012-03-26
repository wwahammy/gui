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
using CoApp.Updater.ViewModel;
using CoApp.Updater.ViewModel.Settings;

namespace CoApp.Updater.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView
    {
        

        public SettingsView()
        {
            InitializeComponent();
        }


        
    }

    public class TabTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = (FrameworkElement)container;
            if (element != null && item != null && item is ScreenViewModel)
            {
                var parent = (FrameworkElement) element.TemplatedParent;
                if (item is UpdateSettingsViewModel)
                {
                    
                    return (DataTemplate)parent.Resources["Update"];
                }
                else if (item is PrivacySettingsViewModel)
                {
                    return (DataTemplate)parent.Resources["Privacy"];
                }
                else if (item is PermissionsSettingsViewModel)
                {
                    return (DataTemplate)parent.Resources["Permissions"];
                }
                else if (item is FeedSettingsViewModel)
                {
                    return (DataTemplate)parent.Resources["Feeds"];
                }
            }

            return null;
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using CoApp.Gui.Toolkit.Controls;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.Gui.Toolkit.ViewModels.Settings;

namespace CoApp.Gui.Toolkit.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : CoAppFrameChild
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

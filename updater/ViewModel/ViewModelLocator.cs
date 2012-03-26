/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:Gui.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using System.Diagnostics.CodeAnalysis;
using CoApp.Updater.ViewModel.Settings;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace CoApp.Updater.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// Use the <strong>mvvmlocatorproperty</strong> snippet to add ViewModels
    /// to this locator.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<PrimaryViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<InstallingViewModel>();
            SimpleIoc.Default.Register<SelectUpdatesViewModel>();
            SimpleIoc.Default.Register<PermissionsSettingsViewModel>();
            SimpleIoc.Default.Register<FeedSettingsViewModel>();
            SimpleIoc.Default.Register<PrivacySettingsViewModel>();
            SimpleIoc.Default.Register<UpdateSettingsViewModel>();
            SimpleIoc.Default.Register<UpdatingViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainWindowViewModel MainWindowViewModel
        {
            get { return MainWindowViewModelStatic; }
        }

        public static MainWindowViewModel MainWindowViewModelStatic
        {
            get { return ServiceLocator.Current.GetInstance<MainWindowViewModel>(); }
        }

        public static PrimaryViewModel PrimaryViewModelStatic
        {
            get { return ServiceLocator.Current.GetInstance<PrimaryViewModel>(); }
        }

        public PrimaryViewModel PrimaryViewModel
        {
            get { return ServiceLocator.Current.GetInstance<PrimaryViewModel>(); }

        }

        public static UpdatingViewModel UpdatingViewModelStatic
        {
            get { return ServiceLocator.Current.GetInstance<UpdatingViewModel>(); }
        }

        public UpdatingViewModel UpdatingViewModel
        {
            get { return UpdatingViewModelStatic; }
        }


        public InstallingViewModel InstallingViewModel

        {
            get { return InstallingViewModelStatic; }
        }

        public static InstallingViewModel InstallingViewModelStatic
        {
            get { return ServiceLocator.Current.GetInstance<InstallingViewModel>(); }
        }

        public SettingsViewModel SettingsViewModel
        {
            get { return SettingsViewModelStatic; }
        }

        public static SettingsViewModel SettingsViewModelStatic
        {
            get { return ServiceLocator.Current.GetInstance<SettingsViewModel>(); }
        }


        public PermissionsSettingsViewModel PermissionsSettingsViewModel
        {
            get { return PermissionsSettingsViewModelStatic; }
        }

        

        public static PermissionsSettingsViewModel PermissionsSettingsViewModelStatic
        {
            get { return ServiceLocator.Current.GetInstance<PermissionsSettingsViewModel>(); }
        }

        public UpdateSettingsViewModel UpdateSettingsViewModel
        {
            get { return UpdateSettingsViewModelStatic; }
        }



        public static UpdateSettingsViewModel UpdateSettingsViewModelStatic
        {
            get { return ServiceLocator.Current.GetInstance<UpdateSettingsViewModel>(); }
        }

        
        public PrivacySettingsViewModel PrivacySettingsViewModel
        {
            get { return PrivacySettingsViewModelStatic; }
        }

        public static PrivacySettingsViewModel PrivacySettingsViewModelStatic
        {
            get { return ServiceLocator.Current.GetInstance<PrivacySettingsViewModel>(); }
        }

        public FeedSettingsViewModel FeedSettingsViewModel
        {
            get { return FeedSettingsViewModelStatic; }
        }

        public static FeedSettingsViewModel FeedSettingsViewModelStatic
        {
            get { return ServiceLocator.Current.GetInstance<FeedSettingsViewModel>(); }
        }

        public static SelectUpdatesViewModel SelectUpdatesViewModelStatic
        {
            get { return ServiceLocator.Current.GetInstance<SelectUpdatesViewModel>(); }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}
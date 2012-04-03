using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoApp.Gui.Toolkit.ViewModels.Settings;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace CoApp.Gui.Toolkit.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<PermissionsSettingsViewModel>();
            SimpleIoc.Default.Register<FeedSettingsViewModel>();
            SimpleIoc.Default.Register<PrivacySettingsViewModel>();
            SimpleIoc.Default.Register<UpdateSettingsViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
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
    }
}

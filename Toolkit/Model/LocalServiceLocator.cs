using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoApp.Gui.Toolkit.Model.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace CoApp.Gui.Toolkit.Model
{
    public class LocalServiceLocator 
    {
        static LocalServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);


#if SAMPLEDATA
            SimpleIoc.Default.Register<IPolicyService, PolicyService>();
            SimpleIoc.Default.Register<INavigationService, NavigationService>();

        

            SimpleIoc.Default.Register<ICoAppService, Sample.CoAppServiceSample>();
            SimpleIoc.Default.Register<IWindowsUserService, WindowsUserService>();
            SimpleIoc.Default.Register<IUpdateSettingsService, UpdateSettingsService>();

          
#else

            SimpleIoc.Default.Register<IPolicyService, PolicyService>();
            SimpleIoc.Default.Register<INavigationService, NavigationService>();
            SimpleIoc.Default.Register<IUpdateSettingsService, UpdateSettingsService>();
            

            SimpleIoc.Default.Register<ICoAppService, CoAppService>();


            
            SimpleIoc.Default.Register<IWindowsUserService, WindowsUserService>();
#endif
        }


        public virtual INavigationService NavigationService
        {
            get { return ServiceLocator.Current.GetInstance<INavigationService>(); }
        }

        

        public virtual ICoAppService CoAppService
        {
            get { return ServiceLocator.Current.GetInstance<ICoAppService>(); }
        }


        public virtual IWindowsUserService WindowsUserService
        {
            get { return ServiceLocator.Current.GetInstance<IWindowsUserService>(); }
        }


        public virtual IPolicyService PolicyService
        {
            get { return ServiceLocator.Current.GetInstance<IPolicyService>(); }
        }

        public virtual IUpdateSettingsService UpdateSettingsService
        {
            get { return ServiceLocator.Current.GetInstance<IUpdateSettingsService>(); }
        }


 
    }
}

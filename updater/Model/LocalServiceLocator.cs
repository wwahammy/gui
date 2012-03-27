using CoApp.Updater.Model.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace CoApp.Updater.Model
{
    public class LocalServiceLocator
    {
        static LocalServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);


#if SAMPLEDATA
            SimpleIoc.Default.Register<IPolicyService, PolicyService>();
            SimpleIoc.Default.Register<INavigationService, NavigationService>();

            SimpleIoc.Default.Register<IUpdateService, UpdateService>();

            SimpleIoc.Default.Register<ICoAppService, Sample.CoAppServiceSample>();
            SimpleIoc.Default.Register<IWindowsUserService, WindowsUserService>();

            SimpleIoc.Default.Register<IAutomationService, AutomationService>();
            SimpleIoc.Default.Register<IInitializeService, InitializeService>();
#else

                    SimpleIoc.Default.Register<IPolicyService, PolicyService>();
                SimpleIoc.Default.Register<INavigationService, NavigationService>();

            SimpleIoc.Default.Register<IUpdateService, UpdateService>();

            SimpleIoc.Default.Register<ICoAppService, CoAppService>();

           
                SimpleIoc.Default.Register<IAutomationService, AutomationService>();
                SimpleIoc.Default.Register<IWindowsUserService, WindowsUserService>();
                 SimpleIoc.Default.Register<IInitializeService, InitializeService>();
#endif
        }


        public virtual INavigationService NavigationService
        {
            get { return ServiceLocator.Current.GetInstance<INavigationService>(); }
        }

        public virtual IUpdateService UpdateService
        {
            get { return ServiceLocator.Current.GetInstance<IUpdateService>(); }
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


        public virtual IAutomationService AutomationService
        {
            get { return ServiceLocator.Current.GetInstance<IAutomationService>(); }
        }


        public virtual IInitializeService InitializeService
        {
            get { return ServiceLocator.Current.GetInstance<IInitializeService>(); }
        }
    }
}
using CoApp.Gui.Toolkit.Model;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Updater.Model.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace CoApp.Updater.Model
{
    public class LocalServiceLocator : CoApp.Gui.Toolkit.Model.LocalServiceLocator
    {
        static LocalServiceLocator() 
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);


#if SAMPLEDATA
         

            SimpleIoc.Default.Register<IUpdateService, UpdateService>();

            SimpleIoc.Default.Register<IAutomationService, AutomationService>();

         
          
#else


            SimpleIoc.Default.Register<IUpdateService, UpdateService>();



           
                SimpleIoc.Default.Register<IAutomationService, AutomationService>();

#endif
        }


      

        public virtual IUpdateService UpdateService
        {
            get { return ServiceLocator.Current.GetInstance<IUpdateService>(); }
        }


      


      




        public virtual IAutomationService AutomationService
        {
            get { return ServiceLocator.Current.GetInstance<IAutomationService>(); }
        }

    }
}
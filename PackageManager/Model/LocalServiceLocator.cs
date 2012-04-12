using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoApp.PackageManager.Model.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace CoApp.PackageManager.Model
{
    public class LocalServiceLocator : Gui.Toolkit.Model.LocalServiceLocator
    {
        static LocalServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<IFeaturedService, FeaturedService>();
        }

        public IFeaturedService FeaturedService
        {
            get { return ServiceLocator.Current.GetInstance<IFeaturedService>(); }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using CoApp.PackageManager.ViewModel.Filter;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace CoApp.PackageManager.ViewModel
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
    public class ViewModelLocator : CoApp.Gui.Toolkit.ViewModels.ViewModelLocator
    {
        private static int _nextSearchVmNumber = 0;
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            //SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<ActivityViewModel>();
            SimpleIoc.Default.Register<ProductViewModel>();
            SimpleIoc.Default.Register<PackageViewModel>();
            SimpleIoc.Default.Register<SearchViewModel>();

        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
       /* [SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainWindowViewModel MainWindowViewModel
        {
            get { return ServiceLocator.Current.GetInstance<MainWindowViewModel>(); }
        }*/


        public HomeViewModel HomeViewModel
        {
            get { return ServiceLocator.Current.GetInstance<HomeViewModel>(); }
        }

        public ActivityViewModel ActivityViewModel
        {
            get { return ServiceLocator.Current.GetInstance<ActivityViewModel>(); }
        }

        public SearchViewModel SearchViewModel
        {
            get { return ServiceLocator.Current.GetInstance<SearchViewModel>(); }
        }
        /*
        public SearchViewModel GetSearchViewModel(string key)
        {
                return ServiceLocator.Current.GetInstance<SearchViewModel>(key);
        }*/

        public SearchViewModel GetSearchViewModel()
        {
            return
                ServiceLocator.Current.GetInstance<SearchViewModel>(Interlocked.Increment(ref _nextSearchVmNumber).ToString(CultureInfo.InvariantCulture));
        }

        public SearchViewModel GetSearchViewModel(SearchRequest request)
        {
            var s = ServiceLocator.Current.GetInstance<SearchViewModel>(Interlocked.Increment(ref _nextSearchVmNumber).ToString(CultureInfo.InvariantCulture));
            s.AddFilterViaRequest(request);
            return s;
        }

        public ProductViewModel GetProductViewModel(string key)
        {
            var ret = ServiceLocator.Current.GetInstance<ProductViewModel>(key);
            ret.InitializationName = key;
            return ret;
        }

        public PackageViewModel GetPackageViewModel(string key)
        {
            var ret = ServiceLocator.Current.GetInstance<PackageViewModel>(key);
            ret.InitializationName = key;
            return ret;
        }

       


        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}
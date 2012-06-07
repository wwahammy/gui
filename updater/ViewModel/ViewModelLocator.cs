

using System.Diagnostics.CodeAnalysis;
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
    public class ViewModelLocator : CoApp.Gui.Toolkit.ViewModels.ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<PrimaryViewModel>();
         
            SimpleIoc.Default.Register<InstallingViewModel>();
            SimpleIoc.Default.Register<SelectUpdatesViewModel>();
      
            SimpleIoc.Default.Register<UpdatingViewModel>();
            SimpleIoc.Default.Register<AskToCreateEventViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainWindowViewModel MainWindowViewModel
        {
            get
            {
                return MainWindowViewModelStatic;
            }
        }

        public static MainWindowViewModel MainWindowViewModelStatic
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }

        public static PrimaryViewModel PrimaryViewModelStatic
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PrimaryViewModel>();
            }


        }

        public PrimaryViewModel PrimaryViewModel
        {
            get { return ServiceLocator.Current.GetInstance<PrimaryViewModel>(); }

        }

        public static UpdatingViewModel UpdatingViewModelStatic
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UpdatingViewModel>();
            }
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

        public AskToCreateEventViewModel AskToCreateEventViewModel
        {
            get { return ServiceLocator.Current.GetInstance<AskToCreateEventViewModel>(); }
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
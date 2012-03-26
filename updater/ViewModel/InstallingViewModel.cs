using System;
using System.Windows.Input;
using CoApp.Updater.Messages;
using CoApp.Updater.Model;
using CoApp.Updater.Model.Interfaces;
using GalaSoft.MvvmLight.Messaging;

namespace CoApp.Updater.ViewModel
{
    public class InstallingViewModel : ScreenViewModel
    {
        private readonly INavigationService nav;
        private readonly IUpdateService update;
        private DateTime? _lastTimeChecked;
        private DateTime? _lastTimeInstalled;
        public InstallingViewModel()
        {
            Messenger.Default.Register<InstallationProgressMessage>(this, HandleMessage);
            MessengerInstance.Register<InstallationFailedMessage>(this, HandleInstallFailed);
            MessengerInstance.Register<InstallationFinishedMessage>(this, HandleInstallFinished);
            var loc = new LocalServiceLocator();
            update = loc.UpdateService;
            nav = loc.NavigationService; 
            Loaded += OnLoaded;
            Title = "CoApp Update";
        }

        private void OnLoaded()
        {
            _percentDone = 0;
            _currentPackageNumber = 0;
            _nameOfCurrentPackage = null;
            _totalPackages = 0;
            // we check for blocks
            
            update.PerformInstallation();
        }


        private void HandleInstallFinished(InstallationFinishedMessage m)
        {
            nav.GoTo(ViewModelLocator.PrimaryViewModelStatic);
        }

        private void HandleInstallFailed(InstallationFailedMessage m)
        {
            nav.GoTo(ViewModelLocator.PrimaryViewModelStatic);
        }


        private double _percentDone;

        public double PercentDone
        {
            get { return _percentDone; }
            set
            {
                _percentDone = value;
                RaisePropertyChanged("PercentDone");
            }
        }

        private int _currentPackageNumber;

        public int CurrentPackageNumber
        {
            get { return _currentPackageNumber; }
            set
            {
                _currentPackageNumber = value;
                RaisePropertyChanged("CurrentPackageNumber");
            }
        }

        private int _totalPackages;

        public int TotalPackages
        {
            get { return _totalPackages; }
            set
            {
                _totalPackages = value;
                RaisePropertyChanged("TotalPackages");
            }
        }

        private string _nameOfCurrentPackage;

        public string NameOfCurrentPackage
        {
            get { return _nameOfCurrentPackage; }
            set
            {
                _nameOfCurrentPackage = value;
                RaisePropertyChanged("NameOfCurrentPackage");
            }
        }


        public DateTime? LastTimeChecked
        {
            get { return _lastTimeChecked; }
            set
            {
                _lastTimeChecked = value;
                RaisePropertyChanged("LastTimeChecked");
            }
        }


        public DateTime? LastTimeInstalled
        {
            get { return _lastTimeInstalled; }
            set
            {
                _lastTimeInstalled = value;
                RaisePropertyChanged("LastTimeInstalled");
            }
        }

        
        private void HandleMessage(InstallationProgressMessage m)
        {
            UpdateOnUI(() =>
                           {
                               CurrentPackageNumber = m.CurrentProductNumber;
                               PercentDone = m.TotalProgressCompleted;
                               NameOfCurrentPackage = m.CurrentProduct.DisplayName;
                               TotalPackages = m.TotalNumberOfProducts;
                               CurrentPackageNumber = m.CurrentProductNumber;
                           });

        }

  
    }
}

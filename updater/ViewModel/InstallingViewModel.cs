using System;
using System.Windows;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.Toolkit.Logging;
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
        internal IAutomationService Automation;
        private int _currentPackageNumber;
        private DateTime? _lastTimeChecked;
        private DateTime? _lastTimeInstalled;
        private string _nameOfCurrentPackage;


        private double _percentDone;
        private int _totalPackages;

        public InstallingViewModel()
        {
            Messenger.Default.Register<InstallationProgressMessage>(this, HandleMessage);
            MessengerInstance.Register<InstallationFailedMessage>(this, HandleInstallFailed);
            MessengerInstance.Register<InstallationFinishedMessage>(this, HandleInstallFinished);
            var loc = new LocalServiceLocator();
            update = loc.UpdateService;
            nav = loc.NavigationService;
            Automation = loc.AutomationService;
            Loaded += OnLoaded;
            Title = "CoApp Update";
        }

        public double PercentDone
        {
            get { return _percentDone; }
            set
            {
                _percentDone = value;
                RaisePropertyChanged("PercentDone");
            }
        }

        public int CurrentPackageNumber
        {
            get { return _currentPackageNumber; }
            set
            {
                _currentPackageNumber = value;
                RaisePropertyChanged("CurrentPackageNumber");
            }
        }

        public int TotalPackages
        {
            get { return _totalPackages; }
            set
            {
                _totalPackages = value;
                RaisePropertyChanged("TotalPackages");
            }
        }

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

        private void OnLoaded()
        {
            _percentDone = 0;
            _currentPackageNumber = 0;
            _nameOfCurrentPackage = null;
            _totalPackages = 0;
            // we check for blocks

            update.PerformInstallation().ContinueWith(t =>
                                                          {
                                                              if (Automation.IsAutomated)
                                                              {
                                                                  Logger.Message("Shutting down" + Environment.NewLine);
                                                                  Application.Current.Dispatcher.Invoke(
                                                                      new Action(() => Application.Current.Shutdown()));

                                                              }
                                                              else
                                                              {
                                                                  nav.GoTo(ViewModelLocator.PrimaryViewModelStatic);
                                                              }
                                                          });
        }


        private void HandleInstallFinished(InstallationFinishedMessage m)
        {
            nav.GoTo(ViewModelLocator.PrimaryViewModelStatic);
        }

        private void HandleInstallFailed(InstallationFailedMessage m)
        {
            nav.GoTo(ViewModelLocator.PrimaryViewModelStatic);
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
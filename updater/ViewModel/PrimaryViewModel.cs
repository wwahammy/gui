using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CoApp.Updater.Controls;
using CoApp.Updater.Messages;
using CoApp.Updater.Model;
using CoApp.Updater.Model.Interfaces;
using CoApp.Updater.Support.Errors;
using GalaSoft.MvvmLight.Command;

namespace CoApp.Updater.ViewModel
{
    public class PrimaryViewModel : ScreenViewModel
    {
        internal INavigationService NavigationService;
        internal IPolicyService PolicyService;
        internal IUpdateService UpdateService;
        private ErrorObject _error;

        private IDictionary<Product, IEnumerable<string>> _errors;
        private bool _hideInstallButton;
        private DateTime? _lastTimeChecked;
        private DateTime? _lastTimeInstalled;
        private int _numberOfProducts;
        private int _numberOfProductsSelected;
        private IList<string> _warnings;

        public PrimaryViewModel()
        {
            Title = "CoApp Update";
            MessengerInstance.Register<InstallationFailedMessage>(this, HandleInstallFailed);
            MessengerInstance.Register<InstallationFinishedMessage>(this, HandleInstallFinished);
            MessengerInstance.Register<SelectedProductsChangedMessage>(this, HandleChanged);
            Loaded += OnLoaded;
            var loc = new LocalServiceLocator();
            UpdateService = loc.UpdateService;
            PolicyService = loc.PolicyService;
            NavigationService = loc.NavigationService;
            RunAdmin = new RelayCommand(() => { });
            SelectUpdates = new RelayCommand(() => NavigationService.GoTo(ViewModelLocator.SelectUpdatesViewModelStatic));
            Install = new RelayCommand(() => NavigationService.GoTo(ViewModelLocator.InstallingViewModelStatic));
            CheckForUpdates = new RelayCommand(() => NavigationService.GoTo(ViewModelLocator.UpdatingViewModelStatic));

            FeedWarning = new RelayCommand(() => MessengerInstance.Send(new MetroDialogBoxMessage
                                                                            {
                                                                                Title =
                                                                                    "The Following Feeds Could Not Be Reached",
                                                                                Content = Warnings,
                                                                                Buttons =
                                                                                    new ObservableCollection
                                                                                    <ButtonDescription>
                                                                                        {
                                                                                            new ButtonDescription
                                                                                                {
                                                                                                    Title = "Cancel",
                                                                                                    IsCancel = true
                                                                                                }
                                                                                        }
                                                                            }));
        }

        public ICommand ShowScreen { get; set; }
        public ICommand FeedWarning { get; set; }


        public ICommand Install { get; set; }


        public int NumberOfProducts
        {
            get { return _numberOfProducts; }
            set
            {
                _numberOfProducts = value;
                RaisePropertyChanged("NumberOfProducts");
            }
        }

        public int NumberOfProductsSelected
        {
            get { return _numberOfProductsSelected; }
            set
            {
                _numberOfProductsSelected = value;
                RaisePropertyChanged("NumberOfProductsSelected");
            }
        }


        public IEnumerable<string> Warnings
        {
            get { return _warnings; }
            private set
            {
                _warnings = new List<string>(value);
                RaisePropertyChanged("Warnings");
            }
        }


        public IDictionary<Product, IEnumerable<string>> Errors
        {
            get { return _errors; }
            private set
            {
                _errors = new Dictionary<Product, IEnumerable<string>>(value);
                RaisePropertyChanged("Errors");
            }
        }

        public bool HideInstallButton
        {
            get { return _hideInstallButton; }
            set
            {
                _hideInstallButton = value;
                RaisePropertyChanged("HideInstallButton");
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


        public ICommand RunAdmin { get; set; }

        public ICommand SelectUpdates { get; set; }
        public ICommand CheckForUpdates { get; set; }

        public ErrorObject Error
        {
            get { return _error; }
            set
            {
                _error = value;
                RaisePropertyChanged("Error");
            }
        }

        private void OnLoaded()
        {
            ResetUI();
        }


        private void HandleInstallFailed(InstallationFailedMessage m)
        {
            UpdateOnUI(() => Errors = m.ErrorsByProduct);
        }

        private void HandleInstallFinished(InstallationFinishedMessage m)
        {
        }


        private void ResetUI()
        {
            NumberOfProducts = UpdateService.NumberOfProducts;
            NumberOfProductsSelected = UpdateService.NumberOfProductsSelected;
            UpdateService.LastTimeInstalled.ContinueWith(t => UpdateOnUI(() => LastTimeInstalled = t.Result));

            //LastTimeChecked = UpdateService.LastTimeChecked;
            HideInstallButton = NumberOfProductsSelected != 0;
        }

        private void HandleChanged(SelectedProductsChangedMessage obj)
        {
            ResetUI();
        }
    }
}
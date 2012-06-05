using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CoApp.Gui.Toolkit.Controls;
using CoApp.Gui.Toolkit.Messages;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.Toolkit.Extensions;
using CoApp.Updater.Messages;
using CoApp.Updater.Model;
using CoApp.Updater.Model.Interfaces;
using CoApp.Updater.ViewModel.Errors;
using GalaSoft.MvvmLight.Command;

namespace CoApp.Updater.ViewModel
{
    public class PrimaryViewModel : ScreenViewModel
    {
        internal INavigationService NavigationService;
        internal IPolicyService PolicyService;
        internal IUpdateService UpdateService;
        private ScreenViewModel _error;


        private bool _hideInstallButton;

        private DateTime? _lastTimeInstalled;
        private int _numberOfProducts;
        private int _numberOfProductsSelected;
        private bool _showDates;
        private IList<string> _warnings = new List<string>();

        public PrimaryViewModel()
        {
            Title = "CoApp Update";
            MessengerInstance.Register<InstallationFailedMessage>(this, HandleInstallFailed);
            MessengerInstance.Register<InstallationFinishedMessage>(this, HandleInstallFinished);
            MessengerInstance.Register<SelectedProductsChangedMessage>(this, HandleChanged);
            MessengerInstance.Register<PoliciesUpdatedMessage>(this, PoliciesUpdated);
            Loaded += OnLoaded;
            var loc = new LocalServiceLocator();
            UpdateService = loc.UpdateService;
            PolicyService = loc.PolicyService;
            NavigationService = loc.NavigationService;
            RunAdmin = new RelayCommand(() => { });
            SelectUpdates = new RelayCommand(() => NavigationService.GoTo(ViewModelLocator.SelectUpdatesViewModelStatic));
            Install = new RelayCommand(() => NavigationService.GoTo(ViewModelLocator.InstallingViewModelStatic, false));
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


        private bool? _canUpdate;

        public bool? CanUpdate
        {
            get { return _canUpdate; }
            set
            {
                _canUpdate = value;
                RaisePropertyChanged("CanUpdate");
            }
        }

        

        private void PoliciesUpdated(PoliciesUpdatedMessage policiesUpdatedMessage= null)
        {
            PolicyService.CanUpdate.ContinueAlways(
                t => UpdateOnUI(() => CanUpdate = t.Result));
        }

        public ICommand ShowScreen { get; set; }
        public ICommand FeedWarning { get; set; }


        public ICommand Install { get; set; }


        public bool ShowDates
        {
            get { return _showDates; }
            set
            {
                _showDates = value;
                RaisePropertyChanged("ShowDates");
            }
        }


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
                RaisePropertyChanged("ShowFeedFailureButton");
            }
        }

        public bool ShowFeedFailureButton
        {
            get { return Warnings.Any(); }
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

        public ScreenViewModel Error
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
            
            AddPostLoadTask(Task.Factory.StartNew(() => PoliciesUpdated()).ContinueWith(t => ResetUI()));
        }


        private void HandleInstallFailed(InstallationFailedMessage m)
        {
            UpdateOnUI(
                () =>
                Error =
                new UpdateFailureViewModel
                    {
                        InnerException = m.Exceptions.First(),
                        UpdateExceptions = new ObservableCollection<Exception>(m.Exceptions)
                    });
        }

        private void HandleInstallFinished(InstallationFinishedMessage m)
        {
        }


        private void ResetUI()
        {
            NumberOfProducts = UpdateService.NumberOfProducts;
            NumberOfProductsSelected = UpdateService.NumberOfProductsSelected;
            UpdateService.LastTimeInstalled.ContinueWith(t => UpdateOnUI(() => LastTimeInstalled = t.Result));
            ShowDates = LastTimeInstalled != null;
            //LastTimeChecked = UpdateService.LastTimeChecked;
            HideInstallButton = NumberOfProductsSelected != 0;
        }

        private void HandleChanged(SelectedProductsChangedMessage obj)
        {
            ResetUI();
        }
    }
}
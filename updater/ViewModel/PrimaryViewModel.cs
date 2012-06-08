using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CoApp.Gui.Toolkit.Controls;
using CoApp.Gui.Toolkit.Messages;
using CoApp.Gui.Toolkit.Model;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.Toolkit.Extensions;
using CoApp.Toolkit.Logging;
using CoApp.Updater.Messages;
using CoApp.Updater.Model.Interfaces;
using CoApp.Updater.ViewModel.Errors;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LocalServiceLocator = CoApp.Updater.Model.LocalServiceLocator;

namespace CoApp.Updater.ViewModel
{
    public class PrimaryViewModel : ScreenViewModel
    {
        internal INavigationService NavigationService;
        internal IPolicyService PolicyService;
        internal IUpdateService UpdateService;
        internal IAutomationService AutomationService;
        internal IUpdateSettingsService UpdateSettingsService;
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
            
            Loaded += OnLoaded;
            var loc = new LocalServiceLocator();
            UpdateService = loc.UpdateService;
            PolicyService = loc.PolicyService;
            NavigationService = loc.NavigationService;
            AutomationService = loc.AutomationService;
            UpdateSettingsService = loc.UpdateSettingsService;
          
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
            ResetUI();
            AddPostLoadTask(HandleAutomation());
        }

        private Task HandleAutomation()
        {
            return Task.Factory.StartNew(HandleAutomationSynch);
            
        }

        private void HandleAutomationSynch()
        {
            if (AutomationService.IsAutomated)
            {
                UpdateChoice c;
                try
                {
                    var r = UpdateSettingsService.GetTask().Result;
                    c = r.UpdateChoice;
                }
                catch (Exception e)
                {
                    Logger.Warning("Couldn't get update choice {0}, {1}", e.Message, e.StackTrace);
                    c = CoAppService.DEFAULT_UPDATE_CHOICE;
                }

                switch (c)
                {
                    case UpdateChoice.Dont:
                        //shut down
                        Logger.Message("Shutting down" + Environment.NewLine);
                        Application.Current.Dispatcher.Invoke(
                                        new Action(() => Application.Current.Shutdown()));
                        break;
                    case UpdateChoice.Notify:
                        Logger.Message("Notify the user");
                        Messenger.Default.Send(new BalloonToolTipMessage { Message = "To review and install your updates, double click on the CoApp icon in the notification area below",
                                                                           Title = "CoApp has found {0} updates.".format(NumberOfProducts),
                                                                           TimeToDisplay = 5000
                        });
                        break;
                    case UpdateChoice.AutoInstallJustUpdates:
                    case UpdateChoice.AutoInstallAll:
                        NavigationService.GoTo(ViewModelLocator.InstallingViewModelStatic);
                        break;
                }
                
            }
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
using System.Windows;
using System.Windows.Input;
using CoApp.Gui.Toolkit.Controls;
using CoApp.Gui.Toolkit.Messages;
using CoApp.Gui.Toolkit.Model;
using CoApp.Gui.Toolkit.Model.Interfaces;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CoApp.Gui.Toolkit.ViewModels
{
    public class CoAppWindowViewModel : ScreenViewModel
    {
        private readonly INavigationService _nav;
        private bool _canGoBack;
        private MetroDialogBoxMessage _dialogBoxInfo;
        private string _dialogBoxVisualState;
        private ScreenViewModel _mainScreenViewModel;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public CoAppWindowViewModel()
        {
            Messenger.Default.Register<GoToMessage>(this, ActOnNavigate);
            Messenger.Default.Register<MetroDialogBoxMessage>(this, HandleDialogBoxMessage);

            _nav = new LocalServiceLocator().NavigationService;
            Back = new RelayCommand(() => _nav.Back());

            Settings =
                new RelayCommand(
                    () => _nav.GoTo(ViewModelLocator.SettingsViewModelStatic));
            //temporary
            Shutdown = new RelayCommand(() => Application.Current.Shutdown());

            CancelDialog = new RelayCommand(() => UpdateOnUI(() => UpdateOnUI(() => DialogBoxVisualState = "Base")));
        }


        public MetroDialogBoxMessage DialogBoxInfo
        {
            get { return _dialogBoxInfo; }
            set
            {
                _dialogBoxInfo = value;
                RaisePropertyChanged("DialogBoxInfo");
            }
        }


        public ICommand CancelDialog { get; set; }


        public string DialogBoxVisualState
        {
            get { return _dialogBoxVisualState; }
            set
            {
                _dialogBoxVisualState = value;
                RaisePropertyChanged("DialogBoxVisualState");
            }
        }


        public ScreenViewModel MainScreenViewModel
        {
            get { return _mainScreenViewModel; }
            set
            {
                _mainScreenViewModel = value;

                RaisePropertyChanged("MainScreenViewModel");
            }
        }


        public bool CanGoBack
        {
            get { return _canGoBack; }
            set
            {
                _canGoBack = value;
                RaisePropertyChanged("CanGoBack");
            }
        }


        public ICommand Settings { get; set; }


        public ICommand Back { get; set; }


        public ICommand Shutdown { get; set; }

        private void HandleDialogBoxMessage(MetroDialogBoxMessage metroDialogBoxMessage)
        {
            if (metroDialogBoxMessage.Buttons != null)
            {
                foreach (ButtonDescription button in metroDialogBoxMessage.Buttons)
                {
                    if (button.IsCancel)
                    {
                        button.Command = CancelDialog;
                    }
                }
            }

            UpdateOnUI(() => DialogBoxInfo = metroDialogBoxMessage);

            UpdateOnUI(() => DialogBoxVisualState = "Showing");
        }


        private void ActOnNavigate(GoToMessage msg)
        {
            UpdateOnUI(() =>
                           {
                               MainScreenViewModel = msg.Destination;
                               CanGoBack = !_nav.StackEmpty;
                           });
        }
    }
}
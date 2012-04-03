using System;
using System.Windows;
using System.Windows.Input;
using CoApp.Gui.Toolkit.Messages;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.Updater.Messages;
using CoApp.Updater.Model;
using CoApp.Updater.Model.Interfaces;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CoApp.Updater.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    public class MainWindowViewModel : ScreenViewModel
    {
        private bool _canGoBack;
        private ScreenViewModel _mainScreenViewModel;
       
        private INavigationService nav;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainWindowViewModel()
        {

            Messenger.Default.Register<GoToMessage>(this, ActOnNavigate);
            Messenger.Default.Register<MetroDialogBoxMessage>(this, HandleDialogBoxMessage);

            nav = new LocalServiceLocator().NavigationService;
            Back = new RelayCommand(() => nav.Back());

            Settings =
                new RelayCommand(
                    () => nav.GoTo(Gui.Toolkit.ViewModels.ViewModelLocator.SettingsViewModelStatic));
            //temporary
            Shutdown = new RelayCommand(() => Application.Current.Shutdown());

            CancelDialog = new RelayCommand(() => UpdateOnUI(() => UpdateOnUI(() => DialogBoxVisualState = "Base")));
        }


        private MetroDialogBoxMessage _dialogBoxInfo;

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


        private void HandleDialogBoxMessage(MetroDialogBoxMessage metroDialogBoxMessage)
        {
            if (metroDialogBoxMessage.Buttons != null)
            {
                foreach(var button in metroDialogBoxMessage.Buttons)
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


        private string _dialogBoxVisualState;

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


        private void ActOnNavigate(GoToMessage msg)
        {
            UpdateOnUI(() =>
                           {
                               MainScreenViewModel = msg.Destination;
                               CanGoBack = !nav.StackEmpty;
                           });
        }




        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}
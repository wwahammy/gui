using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using CoApp.Gui.Toolkit.Controls;
using CoApp.Gui.Toolkit.Messages;
using CoApp.Gui.Toolkit.Model;
using CoApp.Gui.Toolkit.Model.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;

namespace CoApp.Gui.Toolkit.ViewModels
{
    public abstract class ScreenViewModel : ViewModelBase
    {
        private bool? _canBlock;
        private bool? _canChangeSettings;
        private bool? _canInstall;
        private bool? _canRemove;
        private bool? _canRequire;
        private bool? _canSetSessionFeeds;
        private bool? _canSetSystemFeeds;
        private bool? _canUpdate;
        internal IPolicyService _policyService;
        private string _subTitle;
        private string _title;

        protected ScreenViewModel()
        {
            _policyService = new LocalServiceLocator().PolicyService;

            Loaded += OnLoaded;
            Restart = new RelayCommand(RunRestart);
            DefaultElevate =
                new RelayCommand(
                    () =>
                    MessengerInstance.Send(new MetroDialogBoxMessage
                                               {
                                                   Title = "Restart CoApp Update",
                                                   Content =
                                                       "In order to complete your command, CoApp Update must restart. Would you like to restart CoApp Update?",
                                                   Buttons = new ObservableCollection<ButtonDescription>
                                                                 {
                                                                     new ElevateButtonDescription
                                                                         {Title = "Restart", Command = Restart},
                                                                     new ButtonDescription
                                                                         {Title = "Cancel", IsCancel = true}
                                                                 }
                                               }));
        }

        public ICommand DefaultElevate { get; set; }
        public ICommand Restart { get; set; }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        public string SubTitle
        {
            get { return _subTitle; }
            set
            {
                _subTitle = value;
                RaisePropertyChanged("SubTitle");
            }
        }

        public bool? CanUpdate
        {
            get { return _canUpdate; }
            set
            {
                _canUpdate = value;
                RaisePropertyChanged("CanUpdate");
            }
        }


        public bool? CanBlock
        {
            get { return _canBlock; }
            set
            {
                _canBlock = value;
                RaisePropertyChanged("CanBlock");
            }
        }


        public bool? CanRequire
        {
            get { return _canRequire; }
            set
            {
                _canRequire = value;
                RaisePropertyChanged("CanRequire");
            }
        }

        public bool? CanSetSystemFeeds
        {
            get { return _canSetSystemFeeds; }
            set
            {
                _canSetSystemFeeds = value;
                RaisePropertyChanged("CanSetSystemFeeds");
            }
        }

        public bool? CanSetSessionFeeds
        {
            get { return _canSetSessionFeeds; }
            set
            {
                _canSetSessionFeeds = value;
                RaisePropertyChanged("CanSetSessionFeeds");
            }
        }


        public bool? CanRemove
        {
            get { return _canRemove; }
            set
            {
                _canRemove = value;
                RaisePropertyChanged("CanRemove");
            }
        }


        public bool? CanInstall
        {
            get { return _canInstall; }
            private set
            {
                _canInstall = value;
                RaisePropertyChanged("CanInstall");
            }
        }

        public bool? CanChangeSettings
        {
            get { return _canChangeSettings; }
            set
            {
                _canChangeSettings = value;
                RaisePropertyChanged("CanChangeSettings");
            }
        }

        private void RunRestart()
        {
            var startInfo = new ProcessStartInfo(Process.GetCurrentProcess().MainModule.FileName) { Verb = "runas"};


            Process.Start(startInfo);

            Application.Current.Dispatcher.Invoke(new Action(() => Application.Current.Shutdown()));
        }

        private void OnLoaded()
        {
            _policyService.CanUpdate.ContinueWith(
                t => UpdateOnUI(() => CanUpdate = t.Result));
            _policyService.CanBlock.ContinueWith(
                t => UpdateOnUI(() => CanBlock = t.Result));
            _policyService.CanRequire.ContinueWith(
                t => UpdateOnUI(() => CanRequire = t.Result));
            _policyService.CanSetSystemFeeds.ContinueWith(
                t => UpdateOnUI(() => CanSetSystemFeeds = t.Result));
            _policyService.CanSetSessionFeeds.ContinueWith(
                t => UpdateOnUI(() => CanSetSessionFeeds = t.Result));
            _policyService.CanRemove.ContinueWith(
                t => UpdateOnUI(() => CanRemove = t.Result));
            _policyService.CanInstall.ContinueWith(
                t => UpdateOnUI(() => CanInstall = t.Result));
            _policyService.CanChangeSettings.ContinueWith(
                t => UpdateOnUI(() => CanChangeSettings = t.Result));
        }


        public virtual void LoadOnRestart()
        {
        }


// ReSharper disable InconsistentNaming
        protected static void UpdateOnUI(Action action)
// ReSharper restore InconsistentNaming
        {
            if (DispatcherHelper.UIDispatcher == null)
            {
                DispatcherHelper.CheckBeginInvokeOnUI(action.Invoke);
            }
            else
            {
                action.Invoke();
            }
        }


        public virtual void FireLoad()
        {
            if (Loaded != null)
                Loaded();
        }

        public virtual void FireUnload()
        {
            if (Unloaded != null)
                Unloaded();
        }

        public event Action Loaded;

        public event Action Unloaded;


        private ScreenWidth _screenWidth = ScreenWidth.Standard;

        public ScreenWidth ScreenWidth
        {
            get { return _screenWidth; }
            set
            {
                _screenWidth = value;
                RaisePropertyChanged("ScreenWidth");
            }
        }

        
     

        

        /*
        public abstract XElement Serialize();

        public abstract void Deserialize(XElement element);
        */
    }

    public enum ScreenWidth
    {
        Standard,
        FullWidth
    }
}
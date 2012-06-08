using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CoApp.Gui.Toolkit.Controls;
using CoApp.Gui.Toolkit.Messages;
using CoApp.Gui.Toolkit.Model;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Toolkit.Extensions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;

namespace CoApp.Gui.Toolkit.ViewModels
{
    public abstract class ScreenViewModel : ViewModelBase
    {
        private readonly object _postLoadLock = new object();
        private ObservableCollection<object> _additionalHeaderItems;

        internal IPolicyService _policyService;
        private ScreenWidth _screenWidth = ScreenWidth.Standard;
        private string _subTitle;
        private string _title;

        public bool NotInABlock = true;

        protected ScreenViewModel()
        {
            _policyService = new LocalServiceLocator().PolicyService;
            PostLoadTasks = new List<Task>();
            

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            //Restart = new RelayCommand(RunRestart);
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

        private void OnUnloaded()
        {
            MessengerInstance.Unregister<PoliciesUpdatedMessage>(this);
        }

        private void PoliciesUpdated(PoliciesUpdatedMessage policiesUpdatedMessage)
        {
            ReloadPolicies();
        }

        protected List<Task> PostLoadTasks { get; private set; }

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

       

        public ScreenWidth ScreenWidth
        {
            get { return _screenWidth; }
            set
            {
                _screenWidth = value;
                RaisePropertyChanged("ScreenWidth");
            }
        }

        public ObservableCollection<object> AdditionalHeaderItems
        {
            get { return _additionalHeaderItems; }
            set
            {
                _additionalHeaderItems = value;
                RaisePropertyChanged("AdditionalHeaderItems");
            }
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

        private bool? _canInstall;

        public bool? CanInstall
        {
            get { return _canInstall; }
            set
            {
                _canInstall = value;
                RaisePropertyChanged("CanInstall");
            }
        }

        private bool? _canRemove;

        public bool? CanRemove
        {
            get { return _canRemove; }
            set
            {
                _canRemove = value;
                RaisePropertyChanged("CanRemove");
            }
        }


        private bool? _canChangeSettings;

        public bool? CanChangeSettings
        {
            get { return _canChangeSettings; }
            set
            {
                _canChangeSettings = value;
                RaisePropertyChanged("CanChangeSettings");
            }
        }

        private bool? _canSetSessionFeeds;

        public bool? CanSetSessionFeeds
        {
            get { return _canSetSessionFeeds; }
            set
            {
                _canSetSessionFeeds = value;
                RaisePropertyChanged("CanSetSessionFeeds");
            }
        }

        private bool? _canSetSystemFeeds;

        public bool? CanSetSystemFeeds
        {
            get { return _canSetSystemFeeds; }
            set
            {
                _canSetSystemFeeds = value;
                RaisePropertyChanged("CanSetSystemFeeds");
            }
        }

        private bool? _canSetState;

        public bool? CanSetState
        {
            get { return _canSetState; }
            set
            {
                _canSetState = value;
                RaisePropertyChanged("CanSetState");
            }
        }

        protected void  ReloadPolicies()
        {
            _policyService.CanUpdate.ContinueWith(
                 t => UpdateOnUI(() => CanUpdate = t.Result));

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
            _policyService.CanSetState.ContinueWith(
                t => UpdateOnUI(() => CanSetState = t.Result));
        }


        private void OnLoaded()
        {
            ReloadPolicies();
            MessengerInstance.Register<PoliciesUpdatedMessage>(this, PoliciesUpdated);
      
        }


// ReSharper disable InconsistentNaming
        protected static void UpdateOnUI(Action action)
// ReSharper restore InconsistentNaming
        {
            if (DispatcherHelper.UIDispatcher != null)
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
            MessengerInstance.Send(new StartLoadingPageMessage());
            if (Loaded != null)
                Loaded();

            //after loading is finished
            Task task = Task.Factory.StartNew(() =>
                                                  {
                                                      PostLoadTasks.ContinueAlways(
                                                          (tasks) =>
                                                              {
                                                                  ClearPostLoadTasks();
                                                                  MessengerInstance.Send(new EndLoadingPageMessage());
                                                                  FirePostLoadFinished();
                                                              }
                                                          );
                                                  });
        }

        private void FirePostLoadFinished()
        {
            if (PostLoadFinished != null)
                PostLoadFinished();
        }

        public virtual void FireUnload()
        {
            if (Unloaded != null)
                Unloaded();
        }

        public event Action Loaded;

        public event Action Unloaded;

        public event Action PostLoadFinished;


        public void AddPostLoadTask(Task task)
        {
            lock (_postLoadLock)
            {
                PostLoadTasks.Add(task);
            }
        }

        private void ClearPostLoadTasks()
        {
            lock (_postLoadLock)
            {
                PostLoadTasks.Clear();
            }
        }

        protected ICommand CreateBlockingCommand(Action a)
        {
            return null;
        }
    }

    public enum ScreenWidth
    {
        Standard,
        FullWidth
    }
}
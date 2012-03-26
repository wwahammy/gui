using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using CoApp.Updater.Model;
using CoApp.Updater.Model.Interfaces;

namespace CoApp.Updater.ViewModel
{
    public class UpdatingViewModel : ScreenViewModel
    {
        internal IInitializeService InitializeService;
        internal INavigationService NavigationService;
        internal IUpdateService UpdateService;
        private DateTime? _lastTimeChecked;
        private DateTime? _lastTimeInstalled;
        private bool _showDates;
        private CancellationTokenSource _src = new CancellationTokenSource();

        public UpdatingViewModel()
        {
            Title = "CoApp Update";

            var loc = new LocalServiceLocator();
            UpdateService = loc.UpdateService;
            NavigationService = loc.NavigationService;
            InitializeService = loc.InitializeService;
            Loaded += HandleLoaded;
            Unloaded += OnUnloaded;
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


        public bool ShowDates
        {
            get { return _showDates; }
            set
            {
                _showDates = value;
                RaisePropertyChanged("ShowDates");
            }
        }

        private void OnUnloaded()
        {
            _src.Cancel();
        }

        private void HandleLoaded()
        {
            _src = new CancellationTokenSource();


            UpdateService.LastTimeInstalled.ContinueWith(t => UpdateOnUI(() => LastTimeInstalled = t.Result));

            ShowDates = LastTimeChecked != null && LastTimeInstalled != null;

            UpdateService.CheckForUpdates(_src.Token).ContinueWith(
                t =>
                    {
                        if (!InitializeService.Initialized)
                        {
                            InitializeService.RunInitializationAction();
                        }
                        else
                        {
                            NavigationService.GoTo(ViewModelLocator.PrimaryViewModelStatic);
                        }
                    }
                , TaskContinuationOptions.NotOnCanceled);
        }

        public XElement Serialize()
        {
            var root = new XElement("UpdatingViewModel");
            return root;
        }

        public void Deserialize(XElement element)
        {
            //nothing to do
            return;
        }
    }
}
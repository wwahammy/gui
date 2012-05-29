using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.Toolkit.Logging;
using CoApp.Updater.Model;
using CoApp.Updater.Model.Interfaces;

namespace CoApp.Updater.ViewModel
{
    public class UpdatingViewModel : ScreenViewModel
    {
        internal INavigationService NavigationService;
        internal IUpdateService UpdateService;
        internal IAutomationService AutomationService;
 
        private DateTime? _lastTimeInstalled;
        private bool _showDates;
        private CancellationTokenSource _src = new CancellationTokenSource();

        public UpdatingViewModel()
        {
            Title = "CoApp Update";

            var loc = new LocalServiceLocator();
            UpdateService = loc.UpdateService;
            NavigationService = loc.NavigationService;
            AutomationService = loc.AutomationService;
            Loaded += HandleLoaded;
            Unloaded += OnUnloaded;
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

            ShowDates = LastTimeInstalled != null;

            UpdateService.CheckForUpdates(_src.Token).ContinueWith(
                t =>
                    {
                        if (!t.IsCanceled)
                        {
                            if (AutomationService.IsAutomated)
                            {
                                if (UpdateService.NumberOfProductsSelected > 0)
                                {
                                    NavigationService.GoTo(ViewModelLocator.InstallingViewModelStatic);
                                }
                                else
                                {
                                    Logger.Message("Shutting down" + Environment.NewLine);
                                    Application.Current.Dispatcher.Invoke(
                                        new Action(() => Application.Current.Shutdown()));
                                }
                            }
                            else
                            {
                                Logger.Message("We Will Navigate to Primary" + Environment.NewLine);
                                NavigationService.GoTo(ViewModelLocator.PrimaryViewModelStatic);
                            }
                        }

                    });
        }
    }
}
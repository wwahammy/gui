using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CoApp.Gui.Toolkit.Model;
using CoApp.Gui.Toolkit.Model.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace CoApp.Gui.Toolkit.ViewModels.Settings
{
    public class FeedSettingsViewModel : ScreenViewModel
    {
        internal ICoAppService CoAppService;

        private Task<IEnumerable<string>> SysFeed;
        private string _feedUrlToAdd;
        private ObservableCollection<string> _feeds;
        private string _selectedItem;

        private bool NotInABlockingCommand = true;

        public FeedSettingsViewModel()
        {
            Title = "Feeds";
            CoAppService = new LocalServiceLocator().CoAppService;
            Add =
                new RelayCommand<string>(
                    (feedUrl) =>
                        {

                            RaiseAllCanExecutesChanged(false);
                            CoAppService.AddSystemFeed(feedUrl).ContinueWith((t) => ReloadFeeds()).ContinueWith(
                                t => RaiseAllCanExecutesChanged(true));
                        });
            Remove =
                new RelayCommand<string>(
                    (feedUrl) =>
                        {
                            RaiseAllCanExecutesChanged(false);
                            CoAppService.RemoveSystemFeed(feedUrl).ContinueWith((t) => ReloadFeeds()).ContinueWith(
                                t => 
                                    RaiseAllCanExecutesChanged(true));

                        });

            ElevateAdd = new RelayCommand<string>((s) => CoAppService.Elevate().ContinueWith(t => ReloadPolicies()).ContinueWith((t) => Add.Execute(s)));
            ElevateRemove = new RelayCommand<string>((s) => CoAppService.Elevate().ContinueWith((t) => Remove.Execute(s)));
            Loaded += ReloadFeeds;
        }

        private void RaiseAllCanExecutesChanged(bool notInABlockingCommand)
        {
            NotInABlockingCommand = notInABlockingCommand;
           UpdateOnUI(() =>  Add.RaiseCanExecuteChanged());
            UpdateOnUI(() =>  Remove.RaiseCanExecuteChanged());
        }


        public ObservableCollection<string> Feeds
        {
            get { return _feeds; }
            set
            {
                _feeds = value;
                RaisePropertyChanged("Feeds");
            }
        }

        protected override Task ReloadPolicies()
        {
            return
                _policyService.CanChangeSettings.ContinueWith(t => 
                    UpdateOnUI(() => 
                        CanChangeSettings = t.Result)).
                    ContinueWith(
                        t1 =>
                        _policyService.CanSetSystemFeeds.ContinueWith(
                            t => UpdateOnUI(() => 
                                CanSetSystemFeeds = t.Result)))
                ;
        }


        public RelayCommand<string> Add { get; set; }

        public RelayCommand<string> ElevateAdd { get; set; } 


        public RelayCommand<string> Remove { get; set; }
        
        public RelayCommand<string> ElevateRemove { get; set; }


        public string FeedUrlToAdd
        {
            get { return _feedUrlToAdd; }
            set
            {
                _feedUrlToAdd = value;
                RaisePropertyChanged("FeedUrlToAdd");
            }
        }

        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }

        private void ReloadFeeds()
        {
            ReloadPolicies().Wait();
            SysFeed = CoAppService.SystemFeeds;
            SysFeed.ContinueWith((t) =>
                                 UpdateOnUI(() =>
                                            Feeds = new ObservableCollection<string>(t.Result)));
        }
    }
}
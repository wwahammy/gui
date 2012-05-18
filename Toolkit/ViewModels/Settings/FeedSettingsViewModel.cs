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

        public FeedSettingsViewModel()
        {
            Title = "Feeds";
            CoAppService = new LocalServiceLocator().CoAppService;
            Add =
                new RelayCommand<string>(
                    (feedUrl) => CoAppService.AddSystemFeed(feedUrl).ContinueWith((t) => ReloadFeeds()));
            Remove =
                new RelayCommand<string>(
                    (feedUrl) => CoAppService.RemoveSystemFeed(feedUrl).ContinueWith((t) => ReloadFeeds()));
            Loaded += ReloadFeeds;
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


        public ICommand Add { get; set; }


        public ICommand Remove { get; set; }


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
            SysFeed = CoAppService.SystemFeeds;
            SysFeed.ContinueWith((t) =>
                                 UpdateOnUI(() =>
                                            Feeds = new ObservableCollection<string>(t.Result)));
        }
    }
}
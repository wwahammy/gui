using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CoApp.Gui.Toolkit.Model;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Packaging.Client;
using CoApp.Toolkit.Extensions;
using GalaSoft.MvvmLight.Command;
using System.Linq;

namespace CoApp.Gui.Toolkit.ViewModels.Settings
{
    public class FeedSettingsViewModel : ScreenViewModel
    {
        internal ICoAppService CoAppService;

        private Task<IEnumerable<Feed>> SysFeed;
        private string _feedUrlToAdd;
        private ObservableCollection<string> _feeds;
        private string _selectedItem;

     

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

            //if can't elevate, do something
            ElevateAdd = new RelayCommand<string>((s) => CoAppService.Elevate().ContinueAlways(t => ReloadPolicies()).ContinueWith((t) => Add.Execute(s)));
            ElevateRemove = new RelayCommand<string>((s) => CoAppService.Elevate().ContinueWith(t => ReloadPolicies()).ContinueWith((t) => Remove.Execute(s)));
            Loaded += ReloadFeeds;
        }

        private void RaiseAllCanExecutesChanged(bool notInABlockingCommand)
        {
          //  NotInABlockingCommand = notInABlockingCommand;
          // UpdateOnUI(() =>  Add.RaiseCanExecuteChanged());
            //UpdateOnUI(() =>  Remove.RaiseCanExecuteChanged());
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
            
            SysFeed = CoAppService.SystemFeeds;
            SysFeed.ContinueWith((t) =>
                                 UpdateOnUI(() =>
                                            Feeds = new ObservableCollection<string>(t.Result.Select(f => f.Location))));
        }
    }
}
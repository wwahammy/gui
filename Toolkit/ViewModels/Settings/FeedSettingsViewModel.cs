using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using CoApp.Gui.Toolkit.Model;
using CoApp.Gui.Toolkit.Model.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace CoApp.Gui.Toolkit.ViewModels.Settings
{
 
    
    public class FeedSettingsViewModel : ScreenViewModel
    {
        internal ICoAppService CoAppService;

        private ObservableCollection<string> _feeds;
        private Task<IEnumerable<string>> SysFeed;
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

        private void ReloadFeeds()
        {

            SysFeed = CoAppService.SystemFeeds;
                SysFeed.ContinueWith((t) => 
                UpdateOnUI(() => 
                    
                    Feeds = new ObservableCollection<string>( t.Result)));
        }

        private string _feedUrlToAdd;

        
        public string FeedUrlToAdd
        {
            get { return _feedUrlToAdd; }
            set
            {
                _feedUrlToAdd = value;
                RaisePropertyChanged("FeedUrlToAdd");
            }
        }

        private string _selectedItem;
        
        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }

        


        public  XElement Serialize()
        {
            var root= new XElement("FeedSettingsViewModel");
            if (SelectedItem != null)
                root.SetAttributeValue("SelectedItem", SelectedItem);
            if (FeedUrlToAdd != null)
                root.SetAttributeValue("FeedUrlToAdd", FeedUrlToAdd);

            return root;
        }

        public  void Deserialize(XElement element)
        {
            if (element.Name == "FeedSettingsViewModel")
            {

                XAttribute attrib = null;
                if ((attrib = element.Attribute("SelectedItem")) != null)
                {
                    SelectedItem = attrib.Value;
                }

                attrib = null;
                if ((attrib = element.Attribute("FeedUrlToAdd")) != null)
                {
                    FeedUrlToAdd = attrib.Value;
                }
            }


        }
    }
}
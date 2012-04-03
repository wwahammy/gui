using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Linq;
using CoApp.Gui.Toolkit.ViewModels.Settings;
using GalaSoft.MvvmLight.Command;

namespace CoApp.Gui.Toolkit.ViewModels
{
    [Serializable]
    
    public class SettingsViewModel : ScreenViewModel
    {


        public SettingsViewModel()
        {
            TabViewModels = new ObservableCollection<ScreenViewModel> { Feeds, Permissions, Update, Privacy};

            Title = "Settings";
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            TabChanged = new RelayCommand<string>((elementName) =>
                                                      {
                                                          switch (elementName)
                                                          {
                                                              case "FeedsTab":
                                                                  UpdateOnUI(() => SelectedScreen = Feeds);
                                                                  break;
                                                              case "PrivacyTab":
                                                                  UpdateOnUI(() => SelectedScreen = Privacy);
                                                                  break;
                                                              case "PermissionsTab":
                                                                  UpdateOnUI(() => SelectedScreen = Permissions);
                                                                  break;
                                                              case "UpdateTab":
                                                                  UpdateOnUI(() => SelectedScreen = Update);
                                                                  break;
                                                          }
                                                          
                                                      }
                );
            

            SelectedScreen = Feeds;

        }


        

        


        private ObservableCollection<ScreenViewModel> _tabViewModels = new ObservableCollection<ScreenViewModel> {};
        
        public ObservableCollection<ScreenViewModel> TabViewModels
        {
            get { return _tabViewModels; }
            set
            {
                _tabViewModels = value;
                RaisePropertyChanged("TabViewModels");
            }
        }

        
        
        public ICommand TabChanged { get; set; }

        
        public FeedSettingsViewModel Feeds
        {
            get { return ViewModelLocator.FeedSettingsViewModelStatic; }
        }

        
        public PrivacySettingsViewModel Privacy
        {
            get { return ViewModelLocator.PrivacySettingsViewModelStatic; }
        }

        
        public UpdateSettingsViewModel Update
        {
            get { return ViewModelLocator.UpdateSettingsViewModelStatic; }
        }

        
        public PermissionsSettingsViewModel Permissions
        {
            get { return ViewModelLocator.PermissionsSettingsViewModelStatic; }
           
        }

        private ScreenViewModel _selectedScreen;
        
        
        public ScreenViewModel SelectedScreen
        {
            get { return _selectedScreen; }
            set
            {
                _selectedScreen = value;
                
                RaisePropertyChanged("SelectedScreen");
                if (_selectedScreen != null)
                {
                    SubTitle = _selectedScreen.Title;
                }
            }
        }

        

        private void OnUnloaded()
        {
            Feeds.FireUnload();
            Privacy.FireUnload();
            Update.FireUnload();
            Permissions.FireUnload();
        }

        private void OnLoaded()
        {
            Feeds.FireLoad();
            Privacy.FireLoad();
            Update.FireLoad();
            Permissions.FireLoad();
        }

        public XElement Serialize()
        {
            var root = new XElement("SettingsViewModel");
            var selectedScreen = new XElement("SelectedScreen", SelectedScreen.GetType().Name);
            root.Add(selectedScreen);

            return root;
        }

        public void Deserialize(XElement element)
        {
            //nothing to do
            return;
        }
    }
}
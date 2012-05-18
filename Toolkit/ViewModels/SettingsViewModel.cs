using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CoApp.Gui.Toolkit.ViewModels.Settings;
using GalaSoft.MvvmLight.Command;

namespace CoApp.Gui.Toolkit.ViewModels
{
    
    public class SettingsViewModel : ScreenViewModel
    {
        private ScreenViewModel _selectedScreen;
        private ObservableCollection<ScreenViewModel> _tabViewModels = new ObservableCollection<ScreenViewModel> ();

        public SettingsViewModel()
        {
            TabViewModels = new ObservableCollection<ScreenViewModel> {Feeds, Permissions, Update, Privacy};

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
            foreach (ScreenViewModel t in TabViewModels)
            {
                t.FireUnload();
            }
        }

        private void OnLoaded()
        {
            foreach (ScreenViewModel t in TabViewModels)
            {
                var task = new TaskCompletionSource<object>();
                t.PostLoadFinished += () => task.TrySetResult(null);
                AddPostLoadTask(task.Task);
                t.FireLoad();
            }
        }
    }
}
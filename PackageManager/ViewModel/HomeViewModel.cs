using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.PackageManager.Model;
using CoApp.PackageManager.Model.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CoApp.PackageManager.ViewModel
{
    public class HomeViewModel : ScreenViewModel
    {
        internal IFeaturedService Featured;
        internal ICoAppService CoApp;

        private ObservableCollection<PanoramaItemSource> _panoramaItems = new ObservableCollection<PanoramaItemSource>();

        public HomeViewModel()
        {
            Loaded += OnLoaded;
            var loc = new LocalServiceLocator();
            Featured = loc.FeaturedService;
            CoApp = loc.CoAppService;

            GoToSearch = new RelayCommand(() => loc.NavigationService.GoTo(new ViewModelLocator().SearchViewModel));
        }

        public ICommand GoToSearch { get; set; }

        public ObservableCollection<PanoramaItemSource> PanoramaItems
        {
            get
            {
                return _panoramaItems;
            }
            set
            {
                _panoramaItems = value;
                RaisePropertyChanged("PanoramaItems");
            }
        }

        private void OnLoaded()
        {
            Title = "Home";
            ScreenWidth = ScreenWidth.FullWidth;
            PanoramaItems.Clear();

            AddPostLoadTask(
                CoApp.SystemFeeds.ContinueWith(CreateSectionsFromTask));
              
        }

        private void CreateSectionsFromTask(Task<IEnumerable<string>> feedsTask)
        {
            if (feedsTask.IsCanceled || feedsTask.IsFaulted)
            {
                //TODO something bad;
            }

            //if this crashes we're screwed
            foreach (SectionFeature a in feedsTask.Result.Select(f => Featured.GetSectionFeatureForFeed(f).Result))
            {
                var panItem = new PanoramaItemSource
                {
                    Name = a.Name,
                    TopLeft = a.TopLeft,
                    BottomCenter =
                        a.BottomCenter,
                    BottomLeft =
                        a.BottomLeft,
                    BottomRight =
                        a.BottomRight
                };

                UpdateOnUI(() => PanoramaItems.Add(panItem));
            }
        }

    }

    

    public class PanoramaItemSource : ViewModelBase
    {
        private readonly INavigationService _nav = new LocalServiceLocator().NavigationService;
        private readonly ViewModelLocator _vm = new ViewModelLocator();
        private ProductInfo _bottomCenter;
        private ProductInfo _bottomLeft;
        private ProductInfo _bottomRight;

        private string _name;
        private ProductInfo _topLeft;

        public PanoramaItemSource()
        {
            GoToProductPage = new RelayCommand<string>(canonicalName => _nav.GoTo(_vm.GetProductViewModel(canonicalName)));
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }


        public ICommand GoToSource { get; set; }

        public ICommand GoToProductPage { get; set; }

        public ProductInfo TopLeft
        {
            get { return _topLeft; }
            set
            {
                _topLeft = value;
                RaisePropertyChanged("TopLeft");
            }
        }


        public ProductInfo BottomLeft
        {
            get { return _bottomLeft; }
            set
            {
                _bottomLeft = value;
                RaisePropertyChanged("BottomLeft");
            }
        }

        public ProductInfo BottomCenter
        {
            get { return _bottomCenter; }
            set
            {
                _bottomCenter = value;
                RaisePropertyChanged("BottomCenter");
            }
        }


        public ProductInfo BottomRight
        {
            get { return _bottomRight; }
            set
            {
                _bottomRight = value;
                RaisePropertyChanged("BottomRight");
            }
        }
    }
}
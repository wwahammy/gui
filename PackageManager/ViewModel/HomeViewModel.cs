using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CoApp.Gui.Toolkit.Controls;
using CoApp.Gui.Toolkit.Messages;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Gui.Toolkit.Support;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.Gui.Toolkit.ViewModels.Modal;
using CoApp.PackageManager.Model;
using CoApp.PackageManager.Model.Interfaces;
using CoApp.PackageManager.ViewModel.Filter;
using CoApp.Packaging.Client;
using CoApp.Toolkit.Extensions;
using CoApp.Toolkit.Logging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CoApp.PackageManager.ViewModel
{
    public class HomeViewModel : CommonGuiViewModel
    {
        internal ICoAppService CoApp;
        internal IFeaturedService Featured;
        private string _newFeed;

        private ObservableCollection<PanoramaItemSource> _panoramaItems = new ObservableCollection<PanoramaItemSource>();
        private string _visualState = HomeState.Waiting.ToString();

        public HomeViewModel()
        {
            var loc = new LocalServiceLocator();
            Featured = loc.FeaturedService;
            CoApp = loc.CoAppService;

            GoToSearch = new RelayCommand(() => loc.NavigationService.GoTo(new ViewModelLocator().SearchViewModel));
            AddFeed = new RelayCommand(ExecuteAddFeed);
            ElevateAddFeed = new RelayCommand(ExecuteElevateAddFeed);
            Loaded += OnLoaded;
        }


        public ICommand GoToSearch { get; set; }

        public ICommand AddFeed { get; set; }
        public ICommand ElevateAddFeed { get; set; }

        public ObservableCollection<PanoramaItemSource> PanoramaItems
        {
            get { return _panoramaItems; }
            set
            {
                _panoramaItems = value;
                RaisePropertyChanged("PanoramaItems");
            }
        }


        public string VisualState
        {
            get { return _visualState; }
            set
            {
                _visualState = value;
                RaisePropertyChanged("VisualState");
            }
        }


        public string NewFeed
        {
            get { return _newFeed; }
            set
            {
                _newFeed = value;
                RaisePropertyChanged("NewFeed");
            }
        }

        private void ExecuteElevateAddFeed()
        {
            Task e = CoApp.Elevate();
            //elevationfailed
            e.ContinueOnFail(ex =>
                {
                    if (ex.InnerException != null)
                        Logger.Warning("Elevate failed {0}, {1}", ex.InnerException.Message,
                                       ex.InnerException.StackTrace);
                    else
                        Logger.Warning("Elevate failed {0}, {1}", ex.Message, ex.StackTrace);
                    Messenger.Default.Send(NotEnoughPermissions("add a feed"));
                });

            e.Continue(() => AddFeed.Execute(null));
        }

        private void ExecuteAddFeed()
        {
            Task sysFeed = CoApp.AddSystemFeed(NewFeed);


            Task<SectionFeature> featuredTask =
                sysFeed.Continue(
                    () => CoApp.SetFeedStale(NewFeed)).ContinueAlways(t => CoApp.GetSystemFeed(NewFeed)).ContinueOnAll(
                        f => Featured.GetSectionFeatureForFeed(f),
                        e => { },
                        () => { });


            //couldn't get feed?
            featuredTask.ContinueOnFail(e => { });

            featuredTask.Continue(sf => CreatePanoramaItemSourceFromSectionFeature(sf)).Continue(
                ps => UpdateOnUI(() => PanoramaItems.Insert(0, ps)));
        }


        private void OnLoaded()
        {
            Title = "Home";
            ScreenWidth = ScreenWidth.FullWidth;

            PanoramaItems.Clear();


            CoApp.SystemFeeds.ContinueOnAll(CreateSectionsFromTask, exception =>
                {
                    Logger.Error(exception);
                    var vm = new BasicModalViewModel
                        {
                            Title =
                                "Couldn't get feeds",
                            Content =
                                "We couldn't get your system feeds. There must be something wrong with your permissions or CoApp."
                        };
                    vm.SetViaButtonDescriptions(new[]
                        {
                            new ButtonDescription
                                {
                                    Title = "Close CoApp Package Manager",
                                    Command = new RelayCommand(() => Application.Current.Shutdown())
                                }
                        });
                    Messenger.Default.Send(
                        new MetroDialogBoxMessage(vm));
                });
        }

        private void CreateSectionsFromTask(IEnumerable<Feed> feeds)
        {
            if (!feeds.Any())
            {
                UpdateOnUI(() => VisualState = HomeState.Failed.ToString());
            }
            
            //
            //if this crashes we're screwed
            foreach (SectionFeature a in Featured.SortFeedsToFinalOrder(feeds).Select(f => Featured.GetSectionFeatureForFeed(f).Result))
            {
                PanoramaItemSource panItem = CreatePanoramaItemSourceFromSectionFeature(a);
                bool needToChangeState = !PanoramaItems.Any();
                UpdateOnUI(() => PanoramaItems.Add(panItem));
                if (needToChangeState)
                {
                    UpdateOnUI(() => VisualState = HomeState.Loaded.ToString());
                }
            }
        }

        


        private PanoramaItemSource CreatePanoramaItemSourceFromSectionFeature(SectionFeature f)
        {
            var panItem = new PanoramaItemSource
                {
                    Name = f.Name,
                    TopLeft = f.TopLeft,
                    BottomCenter = f.BottomCenter,
                    BottomLeft = f.BottomLeft,
                    BottomRight = f.BottomRight
                };
            return panItem;
        }

        #region Nested type: HomeState

        private enum HomeState
        {
            Waiting,
            Loaded,
            Failed
        }

        #endregion
    }

    public class PanoramaItemSource : ViewModelBase
    {
        private readonly INavigationService _nav = new LocalServiceLocator().NavigationService;
        private readonly ViewModelLocator _vm = new ViewModelLocator();

        private ProductInfo _bottomCenter;
        private ProductInfo _bottomLeft;
        private ProductInfo _bottomRight;

        private string _name;
        private int _numberOfItems;
        private ProductInfo _topLeft;
        private SolidColorBrush _topRightColorBrush;
        private SolidColorBrush _topRightForegroundBrush;
        private string _topRightTitle;

        public PanoramaItemSource()
        {
            TopRightColorBrush = CreateBrush();
            TopRightForegroundBrush = ColorManager.CreateTextColorBrush(TopRightColorBrush);

            GoToProductPage =
                new RelayCommand<string>(canonicalName => _nav.GoTo(_vm.GetProductViewModel(canonicalName)));


            GoToSource =
                new RelayCommand(
                    () => _nav.GoTo(_vm.GetSearchViewModel(new SearchRequest {Category = CAT.FeedUrl, Input = Name})));
        }


        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;

                RaisePropertyChanged("Name");
                TopRightTitle = "See all products from " + Name;
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

        public string TopRightTitle
        {
            get { return _topRightTitle; }
            set
            {
                _topRightTitle = value;
                RaisePropertyChanged("TopRightTitle");
            }
        }


        public SolidColorBrush TopRightColorBrush
        {
            get { return _topRightColorBrush; }
            set
            {
                _topRightColorBrush = value;
                RaisePropertyChanged("TopRightColorBrush");
            }
        }

        public SolidColorBrush TopRightForegroundBrush
        {
            get { return _topRightForegroundBrush; }
            set
            {
                _topRightForegroundBrush = value;
                RaisePropertyChanged("TopRightForegroundBrush");
            }
        }


        public int NumberOfItems
        {
            get { return _numberOfItems; }
            set
            {
                _numberOfItems = value;
                RaisePropertyChanged("NumberOfItems");
            }
        }

        private SolidColorBrush CreateBrush()
        {
            SolidColorBrush clone = ColorManager.DefaultBackgroundColor.Clone();
            clone.Freeze();
            return clone;
        }
    }


    public class PanoramaSources : List<PanoramaItemSource>
    {
    }
}
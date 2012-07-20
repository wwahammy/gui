using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Gui.Toolkit.Support;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.PackageManager.Model;
using CoApp.PackageManager.Model.Interfaces;
using CoApp.PackageManager.ViewModel.Filter;
using CoApp.Packaging.Client;
using CoApp.Packaging.Common;
using CoApp.Toolkit.Collections;
using CoApp.Toolkit.Extensions;
using CoApp.Toolkit.Linq;
using GalaSoft.MvvmLight.Command;
using SLE = System.Linq.Expressions;
using CTL = CoApp.Toolkit.Linq;
using CollectionFilter =
    CoApp.Toolkit.Collections.XList
        <
            System.Linq.Expressions.Expression
                <
                    System.Func
                        <System.Collections.Generic.IEnumerable<CoApp.Packaging.Common.IPackage>,
                            System.Collections.Generic.IEnumerable<CoApp.Packaging.Common.IPackage>>>>;


namespace CoApp.PackageManager.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <from>http://stackoverflow.com/questions/3468866/tabcontrol-with-add-new-tab-button</from>
    public class SearchViewModel : CommonGuiViewModel
    {
        //the length of a page
/*
        private const int NUM_TO_PULL = 50;
*/
        internal ICoAppService CoApp;
        internal LocalServiceLocator Loc;
        internal ViewModelLocator Vm;
        //internal IGuiFilterManager Filters;

        private ObservableCollection<FrictionlessFilter> _appliedFilters;


        private Filter<IPackage> _superFilter;

        private FrictionlessSort _frictionlessSort;
        private bool _inMiddleOfSearch;

        private readonly object _cancelModifyLock = new object();


        private ObservableCollection<ProductInfo> _packages;

        private CancellationTokenSource _cancelToken;

        public IList<string> FeedUrls { get; private set; }

        /*
        private ObservableCollection<GUIFilterBase> _possibleFilters = new ObservableCollection<GUIFilterBase>
                                                                           {
                                                                               new FilterOnText
                                                                                   {
                                                                                       Category = CAT.DisplayName,
                                                                                       NiceName = "Name",
                                                                                       NumberOfFilter =
                                                                                           NumOfFilter.Multiple
                                                                                   },
                                                                               new FilterOnBoolean
                                                                                   {
                                                                                       Category = CAT.IsDependency,
                                                                                       NiceName = "Is a dependency",
                                                                                       NumberOfFilter =
                                                                                           NumOfFilter.Single
                                                                                   },
                                                                               new FilterOnBoolean
                                                                                   {
                                                                                       Category = CAT.IsBlocked,
                                                                                       NiceName = "Is blocked",
                                                                                       NumberOfFilter =
                                                                                           NumOfFilter.Single
                                                                                   },
                                                                               new FilterOnBoolean
                                                                                   {
                                                                                       Category = CAT.IsInstalled,
                                                                                       NumberOfFilter =
                                                                                           NumOfFilter.Single,
                                                                                       NiceName = "Is installed"
                                                                                   },
                                                                               new FilterOnBoolean
                                                                                   {
                                                                                       Category = CAT.IsWanted,
                                                                                       NumberOfFilter =
                                                                                           NumOfFilter.Single,
                                                                                       NiceName = "Is wanted"
                                                                                   },
                                                                                new FilterOnText
                                                                                    {
                                                                                        Category = CAT.FeedUrl,
                                                                                        NumberOfFilter = NumOfFilter.Multiple,
                                                                                        NiceName = "Feed URL"
                                                                                    }
                                                                           };*/

        private ObservableCollection<SortDescriptor> _possibleSorts = new ObservableCollection<SortDescriptor>
                                                                          {
                                                                              new SortDescriptor("Name",
                                                                                                 ps => ps.SortBy(p => p.DisplayName),
                                                                                                 ps => ps.SortByDescending(p => p.DisplayName)),
                                                                              new SortDescriptor("Date Published",
                                                                                                 ps => ps.SortBy(p =>
                                                                                                 p.PackageDetails.
                                                                                                     PublishDate),
                                                                                                      ps => ps.SortByDescending(p =>
                                                                                                 p.PackageDetails.
                                                                                                     PublishDate)
                                                                                                     ),

                                                                              new SortDescriptor("Canonical Name",
                                                                                                 ps => ps.SortBy(p => p.CanonicalName),
                                                                                                 ps => ps.SortByDescending(p => p.CanonicalName)),
                                                                              new SortDescriptor("Formal Name",
                                                                                                 ps => ps.SortBy(p => p.Name),
                                                                                                 ps => ps.SortByDescending(p => p.CanonicalName))
                                                                          };


        public FilterManagement Filters { get; private set; }


        public SearchViewModel()
        {
            ScreenWidth = ScreenWidth.FullWidth;
            Loc = new LocalServiceLocator();

            CoApp = Loc.CoAppService;
            Filters = new FilterManagement();
            /*
            Filters.AllFiltersChanged += ReloadAppliedFilters;
            Filters.FilterApplied += (a, b) => UpdateOnUI(() => RaisePropertyChanged("FiltersToShow"));
  */
            Vm = new ViewModelLocator();


            FeedUrls = new XList<string>();

            
            FrictionlessSort = PossibleSorts[0].Create(ListSortDirection.Ascending);

            AddFilter = new RelayCommand<GUIFilterBase>(i =>
                                                            {
                                                               
                                                                    Filters.ApplyFilter(i);



                                                                    ReloadAppliedFilters();
                                                                RaisePropertyChanged("FiltersToShow");

                                                                ApplyFilters();
                                                            });
            RemoveFilter = new RelayCommand<FrictionlessFilter>(i =>
                                                                    {
                                                                        Filters.RemoveFilter(i);
                                                                        RaisePropertyChanged("FiltersToShow");
                                                                        ReloadAppliedFilters();
                                                                        ApplyFilters();
                                                                    });

            Sort = new RelayCommand<SortDescriptor>(i =>
                                                        {
                                                            FrictionlessSort = i.Create(ListSortDirection.Ascending);
                                                            ApplyFilters();
                                                        });

            SortDescending = new RelayCommand<SortDescriptor>(i =>
                                                                  {
                                                                      FrictionlessSort =
                                                                          i.Create(ListSortDirection.Descending);
                                                                      ApplyFilters();
                                                                  });

            
            
            GoToProduct =
                new RelayCommand<ProductInfo>(p => Loc.NavigationService.GoTo(Vm.GetProductViewModel(p.CanonicalName)));
            Loaded += OnLoaded;
            PropertyChanged += CreateSuperFilter;
        }

        private void CreateSuperFilter(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "TypedInput")
            {
                //let's create the filter
                Filter<IPackage> fullFilter = null;
                foreach ( var f in Filters.AllFilters.Where(f => f.IsPartOfSuperFilter).OfType<ISuperFilterCopy>())
                {
                    fullFilter |= f.CreateForSuperFilter(TypedInput).Filter;
                }

                _superFilter = fullFilter;


                ApplyFilters();
            }
        }

        void SetCancelToken()
        {
            lock (_cancelModifyLock)
            {
               if (_cancelToken != null)
               {
                   _cancelToken.Cancel();
               }

                _cancelToken = new CancellationTokenSource();
            }
        }

        public ICommand GoToProduct { get; set; }

        public bool InMiddleOfSearch
        {
            get { return _inMiddleOfSearch; }
            set
            {
                _inMiddleOfSearch = value;
                RaisePropertyChanged("InMiddleOfSearch");
            }
        }

        public ObservableCollection<SortDescriptor> PossibleSorts
        {
            get { return _possibleSorts; }
            set
            {
                _possibleSorts = value;
                RaisePropertyChanged("PossibleSorts");
            }
        }

        public ObservableCollection<FrictionlessFilter> AppliedFilters
        {


            get
            {
                
                if (_appliedFilters == null)
                {
                    _appliedFilters = new ObservableCollection<FrictionlessFilter>(Filters.AppliedFilters);
                    var itemsView = (IEditableCollectionView) CollectionViewSource.GetDefaultView(_appliedFilters);
                    itemsView.NewItemPlaceholderPosition = NewItemPlaceholderPosition.AtEnd;
                }

                return _appliedFilters;
            }
            set
            {
                _appliedFilters = value;
                RaisePropertyChanged("AppliedFilters");
            }
        }


        public ObservableCollection<GUIFilterBase> FiltersToShow
        {
            get
            {
                return
                    new ObservableCollection<GUIFilterBase>(Filters.AvailableFilters);
            }
        }


        public ObservableCollection<ProductInfo> Packages
        {
            get { return _packages; }
            set
            {
                _packages = value;
                RaisePropertyChanged("Packages");
            }
        }


        public FrictionlessSort FrictionlessSort
        {
            get { return _frictionlessSort; }
            set
            {
                _frictionlessSort = value;
                RaisePropertyChanged("FrictionlessSort");
            }
        }

        public RelayCommand<GUIFilterBase> AddFilter { get; set; }
        public ICommand RemoveFilter { get; set; }
        public ICommand Sort { get; set; }
        public ICommand SortDescending { get; set; }
        
        public void AddFilterViaRequest(SearchRequest req)
        {
            var filt = Filters.AvailableFilters.FirstOrDefault(f => f.Category == req.Category);
            if (filt == null)
            {
                
            }
            else
            {
                if (filt is IFilterWithTextInput)
                {
                    ((IFilterWithTextInput) filt).FromInputSetValue(req.Input);
                    AddFilter.Execute(filt);
                }
                
            }
        }

        public new string Title
        {
            get { return string.IsNullOrWhiteSpace(TypedInput) ? "Search" : "Search for " + TypedInput; }
        }


        private string _typedInput = "";

        public string TypedInput
        {
            get { return _typedInput; }
            set
            {
                _typedInput = value;
                RaisePropertyChanged("TypedInput");
                
                RaisePropertyChanged("Title");
            }
        }

        

        private void ApplyFilters()
        {
            
            SetCancelToken();
            //TODO: we should lock AppliedFilters list here so we don't have issues with someone adding one while we grab it
            var a = AppliedFilters.ToArray();
            //create actual filter
            IEnumerable<IGrouping<CAT, FrictionlessFilter>> groups = a.GroupBy(f => f.Category);
            Filter<IPackage> fullFilter = null;
            FeedUrls = new XList<string>();
            foreach (var g in groups)
            {
                if (g.Key == CAT.FeedUrl)
                {
                    //we do something strange since feedUrls are strange stuff
                    FeedUrls = new XList<string>(g.Cast<FeedUrlFilter>().Select(f => f.ChoiceValue));
                }

                Filter<IPackage> groupF = null;
                foreach (var filter in g)
                {
                    if (groupF == null)
                        groupF = filter.Filter;
                    else
                        groupF |= filter.Filter;
                }

                if (fullFilter == null)
                    fullFilter = groupF;
                else
                    fullFilter &= groupF;
            }

            if (_superFilter != null)
            {
                fullFilter &= _superFilter;
            }

            //get only highestpackages
            CollectionFilter collectionFilter = null;
            collectionFilter = collectionFilter.Then(p => p.HighestPackages());
            collectionFilter = collectionFilter.Then(FrictionlessSort.SelectedProperty);


            //create sort


            if (FrictionlessSort.Direction == ListSortDirection.Ascending)
            {
                GetPackages(fullFilter, collectionFilter, ListSortDirection.Ascending);
            }
            else
            {
                GetPackages(fullFilter, collectionFilter, ListSortDirection.Descending);
            }
        }

        private void GetPackages(Filter<IPackage> packageFilter, CollectionFilter collectionFilter,
                                 ListSortDirection direction)
        {
            UpdateOnUI(() => InMiddleOfSearch = true);

            Task<IEnumerable<Package>> task = null;
            if (FeedUrls.IsNullOrEmpty())
            {
                task = CoApp.GetPackages(packageFilter, collectionFilter);
            }

            else
            {
                task = Task.Factory.StartNew(() =>
                                                 {
                                                     var t = FeedUrls.SelectMany(
                                                         s =>
                                                         CoApp.GetPackages(packageFilter, collectionFilter,
                                                                           locationFeed: s).
                                                                           ContinueAlways(t1 =>
                                                                                              {
                                                                                                  if (t1.IsFaulted || t1.IsCanceled)
                                                                                                      return null;
                                                                                                  else
                                                                                                  {
                                                                                                      return t1.Result;
                                                                                                  }
                                                                                              }).Result.Where(i => i != null) );

                                                     return t;


                                                 }, TaskCreationOptions.AttachedToParent);
            }
            task.ContinueAlways (t =>
                                    {

                                        if (!_cancelToken.IsCancellationRequested)
                                        {
                                            IEnumerable<Package> packages =
                                                t.Result;


                                            ProductInfo[] ret;
                                            if (direction ==
                                                ListSortDirection.Ascending)
                                            {
                                                ret = packages.
                                                    Select(
                                                        ProductInfo.FromIPackage)
                                                    .ToArray();
                                            }
                                            else
                                            {
                                                ret =
                                                    packages.
                                                        Select(
                                                            ProductInfo.
                                                                FromIPackage).
                                                        ToArray();
                                            }

                                            var coll = new ObservableCollection
                                                <ProductInfo>(ret);

                                            UpdateOnUI(() =>
                                                       Packages = coll);
                                        }
                                        UpdateOnUI(
                                            () => InMiddleOfSearch = false);
                                    });
        }

        private void ReloadAppliedFilters()
        {
            UpdateOnUI(() => AppliedFilters = null);
           
        }

        private void OnLoaded()
        {
            if (Packages == null)
            {
                ApplyFilters();
            }
        }
    }
}
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.PackageManager.Model;
using CoApp.PackageManager.ViewModel.Filter;
using CoApp.Packaging.Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CoApp.PackageManager.ViewModel
{
    public abstract class PackageProductCommonViewModel : CommonGuiViewModel
    {
        public string InitializationName;
        internal IPolicyService Policy;
        internal INavigationService Nav;
        internal ViewModelLocator VmLoc;
        internal ICoAppService CoApp;

        private double _averageRating;
        private string _bugTrackerUrl;
        private ObservableCollection<ContributorToCommand> _contributors;
        private ObservableCollection<PackageToCommand> _dependencies;
        private string _description;
        private string _displayName;
        private BitmapSource _icon;
        private bool _isInstalled;
        private bool _isSafeForWork = true;
        private int _numberOfRatings;
        private SolidColorBrush _primaryColor;
        private string _publisherName;
        private string _summary;
        private ObservableCollection<TagToCommand> _tags;
        private SolidColorBrush _textColor;
        private int _usersDisagree;


        protected PackageProductCommonViewModel()
        {
            var loc = new LocalServiceLocator();
            Policy = loc.PolicyService;
            Nav = loc.NavigationService;
            CoApp = loc.CoAppService;
            VmLoc = new ViewModelLocator();
            Loaded += OnLoaded;
        }

        public ICommand ReportNSFW { get; set; }

        public bool IsInstalled
        {
            get { return _isInstalled; }
            set
            {
                _isInstalled = value;
                RaisePropertyChanged("IsInstalled");
            }
        }


        public RelayCommand Install { get; set; }
        public RelayCommand Remove { get; set; }

        public RelayCommand ElevateRemove { get; set; }


        public string PublisherName
        {
            get { return _publisherName; }
            set
            {
                _publisherName = value;
                RaisePropertyChanged("PublisherName");
            }
        }

        public string BugTrackerUrl
        {
            get { return _bugTrackerUrl; }
            set
            {
                _bugTrackerUrl = value;
                RaisePropertyChanged("BugTrackerUrl");
            }
        }

        public bool IsSafeForWork
        {
            get { return _isSafeForWork; }
            set
            {
                _isSafeForWork = value;
                RaisePropertyChanged("IsSafeForWork");
            }
        }

        public int UsersDisagree
        {
            get { return _usersDisagree; }
            set
            {
                _usersDisagree = value;
                RaisePropertyChanged("UsersDisagree");
            }
        }

        public BitmapSource Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                RaisePropertyChanged("Icon");
            }
        }

        public int NumberOfRatings
        {
            get { return _numberOfRatings; }
            set
            {
                _numberOfRatings = value;
                RaisePropertyChanged("NumberOfRatings");
            }
        }


        public double AverageRating
        {
            get { return _averageRating; }
            set
            {
                _averageRating = value;
                RaisePropertyChanged("AverageRating");
            }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                RaisePropertyChanged("DisplayName");
            }
        }

        public SolidColorBrush PrimaryColor
        {
            get { return _primaryColor; }
            set
            {
                _primaryColor = value;
                RaisePropertyChanged("PrimaryColor");
            }
        }

        public SolidColorBrush TextColor
        {
            get { return _textColor; }
            set
            {
                _textColor = value;
                RaisePropertyChanged("TextColor");
            }
        }


        public ObservableCollection<PackageToCommand> Dependencies
        {
            get { return _dependencies; }
            set
            {
                _dependencies = value;
                RaisePropertyChanged("Dependencies");
            }
        }


        public ObservableCollection<TagToCommand> Tags
        {
            get { return _tags; }
            set
            {
                _tags = value;
                RaisePropertyChanged("Tags");
            }
        }

        public ObservableCollection<ContributorToCommand> Contributors
        {
            get { return _contributors; }
            set
            {
                _contributors = value;
                RaisePropertyChanged("Contributors");
            }
        }


        public string Summary
        {
            get { return _summary; }
            set
            {
                _summary = value;
                if (String.IsNullOrWhiteSpace(value))
                    _summary = "None Provided";
                RaisePropertyChanged("Summary");
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                if (String.IsNullOrWhiteSpace(value))
                    _description = "None Provided";
                RaisePropertyChanged("Description");
            }
        }

        public RelayCommand ElevateInstall { get; set; }
        public ICommand GoToPublisher { get; set; }

        private void OnLoaded()
        {
            /*
            AddPostLoadTask(Policy.CanInstall.ContinueWith(t =>
                {
                    t.RethrowWhenFaulted();
                    UpdateOnUI(() => CanInstall = t.Result);
                }
                ));
            
            AddPostLoadTask(Policy.CanRemove.ContinueWith(t =>
            {
                t.RethrowWhenFaulted();
                UpdateOnUI(() => CanRemove = t.Result);
            }
                ));*/
        }

        protected void SetTags(IPackage p)
        {
            try
            {
                if (p != null && p.PackageDetails != null && p.PackageDetails.Tags != null)
                {
                    var tags = new ObservableCollection<TagToCommand>(p.PackageDetails.Tags.Select(t => new TagToCommand
                    {
                        Tag
                            =
                            t,
                        Navigate
                            =
                            new RelayCommand
                                                                                                        (
                                                                                                        ()
                                                                                                        =>
                                                                                                        Nav
                                                                                                            .
                                                                                                            GoTo
                                                                                                            (
                                                                                                                VmLoc
                                                                                                                    .
                                                                                                                    GetSearchViewModel
                                                                                                                    (new SearchRequest
                                                                                                                    {
                                                                                                                        Category
                                                                                                                            =
                                                                                                                            CAT
                                                                                                                     .
                                                                                                                     Tag,
                                                                                                                        Input
                                                                                                                            =
                                                                                                                            t
                                                                                                                    })))
                    }));

                    UpdateOnUI(() => Tags = tags);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        protected void SetDependencies(IPackage p)
        {
            try
            {
                if (p != null && p.Dependencies != null)
                {
                    var dep = new ObservableCollection<PackageToCommand>(
                        p.Dependencies.Select(d =>
                                              new PackageToCommand
                                              {
                                                  Package =
                                                      ProductInfo.FromIPackage(
                                                          CoApp.GetPackageDetails(d.CanonicalName).Result),
                                                  Navigate =
                                                      new RelayCommand(
                                              () =>
                                              Nav.GoTo(VmLoc.GetPackageViewModel(d.CanonicalName.ToString())))
                                              }));
                    UpdateOnUI(() => Dependencies = dep);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }


    public class ContributorToCommand : ViewModelBase
    {
        private string _contributor;

        public string Contributor
        {
            get { return _contributor; }
            set
            {
                _contributor = value;
                RaisePropertyChanged("Contributor");
            }
        }

        public ICommand Navigate { get; set; }
    }

    public class TagToCommand : ViewModelBase
    {
        private string _tag;

        public string Tag
        {
            get { return _tag; }
            set
            {
                _tag = value;
                RaisePropertyChanged("Tag");
            }
        }

        public ICommand Navigate { get; set; }
    }

    public class PackageToCommand : ViewModelBase
    {
        private ProductInfo _upper;

        public ProductInfo Package
        {
            get { return _upper; }
            set
            {
                _upper = value;
                RaisePropertyChanged("Dependency");
            }
        }

        public ICommand Navigate { get; set; }
    }
}
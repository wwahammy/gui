using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.PackageManager.Model;
using GalaSoft.MvvmLight;

namespace CoApp.PackageManager.ViewModel
{
    public abstract class PackageProductCommonViewModel : ScreenViewModel
    {
        public string InitializationName;


        private double _averageRating;
        private string _bugTrackerUrl;
        private ObservableCollection<ContributorToCommand> _contributors;
        private ObservableCollection<PackageToCommand> _dependencies;
        private string _description;
        private string _displayName;
        private BitmapSource _icon;
        private ICommand _install;
        private bool _isInstalled;
        private bool _isSafeForWork = true;
        private int _numberOfRatings;
        private string _publisherName;
        private ICommand _remove;
        private string _summary;
        private ObservableCollection<TagToCommand> _tags;
        private int _usersDisagree;
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


        public ICommand Install
        {
            get { return _install; }
            set
            {
                _install = value;
                RaisePropertyChanged("Install");
            }
        }

        public ICommand Remove
        {
            get { return _remove; }
            set
            {
                _remove = value;
                RaisePropertyChanged("Remove");
            }
        }


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
                RaisePropertyChanged("Summary");
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged("Description");
            }
        }


        public ICommand GoToPublisher { get; set; }
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
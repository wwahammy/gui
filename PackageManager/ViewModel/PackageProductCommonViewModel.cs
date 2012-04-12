using System.Collections.ObjectModel;
using System.Windows.Input;
using CoApp.Gui.Toolkit.ViewModels;
using GalaSoft.MvvmLight;

namespace CoApp.PackageManager.ViewModel
{
    public abstract class PackageProductCommonViewModel : ScreenViewModel
    {
        private double _averageRating;
        private string _bugTrackerUrl;
        private byte[] _icon;
        private bool _isSafeForWork;
        private int _numberOfRatings;
        private string _publisherName;
        private int _usersDisagree;

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

        public byte[] Icon
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

        private string _authorName;

        public string AuthorName
        {
            get { return _authorName; }
            set
            {
                _authorName = value;
                RaisePropertyChanged("AuthorName");
            }
        }

        private ObservableCollection<TagToCommand> _tags;

        public ObservableCollection<TagToCommand> Tags
        {
            get { return _tags; }
            set
            {
                _tags = value;
                RaisePropertyChanged("Tags");
            }
        }

        private ObservableCollection<ContributorToCommand> _contributors;

        public ObservableCollection<ContributorToCommand> Contributors
        {
            get { return _contributors; }
            set
            {
                _contributors = value;
                RaisePropertyChanged("Contributors");
            }
        }





        private string _summary;

        public string Summary
        {
            get { return _summary; }
            set
            {
                _summary = value;
                RaisePropertyChanged("Summary");
            }
        }

        private string _description;

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
}
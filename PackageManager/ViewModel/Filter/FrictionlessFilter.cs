using CoApp.Packaging.Common;
using CoApp.Toolkit.Linq;
using GalaSoft.MvvmLight;

namespace CoApp.PackageManager.ViewModel.Filter
{
    public class FrictionlessFilter : ObservableObject
    {
        private string _filterDisplay;

        public string FilterDisplay
        {
            get { return _filterDisplay; }
            set
            {
                _filterDisplay = value;
                RaisePropertyChanged("FilterDisplay");
            }
        }


        private CAT _category;

        public CAT Category
        {
            get { return _category; }
            set
            {
                _category = value;
                RaisePropertyChanged("Category");
            }
        }


        private Filter<IPackage> _filter;

        public Filter<IPackage> Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                RaisePropertyChanged("Filter");
            }
        }



    }


    public class FrictionlessFilter<TChoiceType> :FrictionlessFilter
    {
        private TChoiceType _choiceValue;

        public TChoiceType ChoiceValue
        {
            get { return _choiceValue; }
            set
            {
                _choiceValue = value;
                RaisePropertyChanged("ChoiceValue");
            }
        }
    }

    public class FeedUrlFilter : FrictionlessFilter<string>
    {

        
    }

    public enum CAT
    {
        DisplayName,
        Version,
        IsWanted,
        IsBlocked,
        IsInstalled,
        IsDependency,
        FeedUrl,
        Tag,
        IsActive,
        Description,
        Publisher,
        License,
        Contributor,
        Summary,
        AuthorVersion,
        Stability,
        Architecture,
        Flavor,
        PublishDate

    }


    public enum NumOfFilter
    {
        Single,
        Multiple
    }
}

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.Packaging.Common;
using CoApp.Toolkit.Linq;
using GalaSoft.MvvmLight.Command;
using SLE = System.Linq.Expressions;

namespace CoApp.PackageManager.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <from>http://stackoverflow.com/questions/3468866/tabcontrol-with-add-new-tab-button</from>
    public class SearchViewModel : ScreenViewModel
    {


        private ObservableCollection<SortDescriptor> _possibleSorts = new ObservableCollection<SortDescriptor>
                                                                           {
                                                                               new SortDescriptor("Name", PropertyExpression<IPackage>.Create(p => p.DisplayName))
                                                                           };

        public ObservableCollection<SortDescriptor> PossibleSorts
        {
            get { return _possibleSorts; }
            set
            {
                _possibleSorts = value;
                RaisePropertyChanged("PossibleSorts");
            }
        }

        
        

        private ObservableCollection<FilterModel> _filters = new ObservableCollection<FilterModel>();

        public SearchViewModel()
        {
            AddFilter = new RelayCommand<FilterModel>(i => Filters.Add(i));
            RemoveFilter = new RelayCommand<FilterModel>(i => Filters.Remove(i));
           // Sort = new RelayCommand<SortDescriptor>(i => i.Property.Compile().DynamicInvoke());
            //Sort = new RelayCommand<SortFilter>(i => );
            Loaded += OnLoaded;
        }

        private void OnLoaded()
        {
            if (Packages == null)
            {
                SortTitle = "Name";
                // AddPostLoadTask(Task.Factory.StartNew(() => ));
            }
        }


        private string _sortTitle;

        public string SortTitle
        {
            get { return _sortTitle; }
            set
            {
                _sortTitle = value;
                RaisePropertyChanged("SortTitle");
            }
        }

        private ListSortDirection _sortDirection = ListSortDirection.Ascending;

        public ListSortDirection SortDirection
        {
            get { return _sortDirection; }
            set
            {
                _sortDirection = value;
                RaisePropertyChanged("SortDirection");
            }
        }

        

        private ObservableCollection<IPackage> _packages;

        public ObservableCollection<IPackage> Packages
        {
            get { return _packages; }
            set
            {
                _packages = value;
                RaisePropertyChanged("Packages");
            }
        }

        

        

        public ObservableCollection<FilterModel> Filters
        {
            get
            {
                var view = (IEditableCollectionView) CollectionViewSource.GetDefaultView(_filters);
                view.NewItemPlaceholderPosition = NewItemPlaceholderPosition.AtEnd;
                return _filters;
            }
            set
            {
                _filters = value;
                RaisePropertyChanged("Filters");
            }
        }


        

        public ICommand AddFilter { get; set; }
        public ICommand RemoveFilter { get; set; }
        public ICommand Sort { get; set; }
        public ICommand SortDescending { get; set; }
    }

   

    public class SortDescriptor
    {
        public SortDescriptor(string title, SLE.LambdaExpression property)
        {
            Title = title;
            Property = property;
        }


        public string Title { get; protected set; }
        public SLE.LambdaExpression Property { get; private set; }
    }

    public class SortFilter<TProperty> : SortFilter
    {
        public SortFilter(Func<IPackage, TProperty> propertyFunc, string title, ListSortDirection sortDirection)
        {
            PropertyFunc = propertyFunc;
            Title = title;
        }
        public Func<IPackage, TProperty> PropertyFunc { get; private set; }
    }

    public class SortFilter
    {
        public string Title { get; protected set; }

        

        
    }

    public class FilterModel
    {
        public string Title { get; set; }


    }


    
}
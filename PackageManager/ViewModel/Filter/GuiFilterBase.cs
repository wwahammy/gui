using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace CoApp.PackageManager.ViewModel.Filter
{
    public abstract class GUIFilterBase : ObservableObject
    {
        protected FilterManagement Management;
        protected GUIFilterBase(FilterManagement management)
        {
            Management = management;
        }

        public bool IsPartOfSuperFilter { get; set; }
        

        private CAT _category;
        private string _niceName;
        private NumOfFilter _numberOfFilter;

        public NumOfFilter NumberOfFilter
        {
            get { return _numberOfFilter; }
            set
            {
                _numberOfFilter = value;
                RaisePropertyChanged("NumberOfFilter");
            }
        }

        public string NiceName
        {
            get { return _niceName; }
            set
            {
                _niceName = value;
                RaisePropertyChanged("NiceName");
            }
        }

        public CAT Category
        {
            get { return _category; }
            set
            {
                _category = value;
                RaisePropertyChanged("Category");
            }
        }


        public abstract FrictionlessFilter Create();

        public abstract bool CanBeCreated();
    }
}

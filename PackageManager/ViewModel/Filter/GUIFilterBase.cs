using GalaSoft.MvvmLight;

namespace CoApp.PackageManager.ViewModel.Filter
{
    public abstract class GUIFilterBase : ObservableObject, IFilter
    {
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

        #region IFilter Members

        public abstract FrictionlessFilter Create();

        #endregion
    }
}
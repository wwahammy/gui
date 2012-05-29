using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;

namespace CoApp.PackageManager.ViewModel.Filter
{
    public abstract class GUIFilterBase : ObservableObject, IFilter
    {
        private NumOfFilter _numberOfFilter;
        private string _niceName;
        private CAT _category;

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
    }
}

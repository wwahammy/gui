using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoApp.Packaging.Common;
using GalaSoft.MvvmLight;
using CoApp.Toolkit.Linq;

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

    public enum CAT
    {
        DisplayName,
        Version,
        IsWanted,
        IsBlocked,
        IsInstalled,
        IsDependency
    }


    public enum NumOfFilter
    {
        Single,
        Multiple
    }
}

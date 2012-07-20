using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CoApp.Packaging.Common;
using GalaSoft.MvvmLight;

namespace CoApp.PackageManager.ViewModel.Filter
{

    public class FrictionlessSort : ObservableObject
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }


        public Expression<Func<IEnumerable<IPackage>, IEnumerable<IPackage>>> SelectedProperty
        {
            get { return Direction == ListSortDirection.Ascending ? Ascending : Descending; }
        }


        private Expression<Func<IEnumerable<IPackage>, IEnumerable<IPackage>>> _ascending;

        public Expression<Func<IEnumerable<IPackage>, IEnumerable<IPackage>>> Ascending
        {
            get { return _ascending; }
            set
            {
                _ascending = value;
                RaisePropertyChanged("Ascending");
            }
        }


        private Expression<Func<IEnumerable<IPackage>, IEnumerable<IPackage>>> _descending;

        public Expression<Func<IEnumerable<IPackage>, IEnumerable<IPackage>>> Descending
        {
            get { return _descending; }
            set
            {
                _descending = value;
                RaisePropertyChanged("Descending");
            }
        }


        private ListSortDirection _direction;

        public ListSortDirection Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                RaisePropertyChanged("Direction");
            }
        }
    }
}

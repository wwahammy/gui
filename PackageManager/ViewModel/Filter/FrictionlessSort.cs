using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using GalaSoft.MvvmLight;

namespace CoApp.PackageManager.ViewModel.Filter
{

    public class FrictionlessSort<TInput, TOutput> : FrictionlessSort<TInput>
    {

        private Expression<Func<TInput, TOutput>> _property;

        public new Expression<Func<TInput, TOutput>> Property
        {
            get { return _property; }
            set
            {
                _property = value;
                RaisePropertyChanged("Property");
            }
        }
    }

    public class FrictionlessSort<TInput> : FrictionlessSort
    {

        private Expression<Func<TInput, dynamic>> _property;

        public new Expression<Func<TInput, dynamic>> Property
        {
            get { return _property; }
            set
            {
                _property = value;
                RaisePropertyChanged("Property");
            }
        }
    }


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

        private Expression<Func<dynamic, dynamic>> _property;

        public Expression<Func<dynamic, dynamic>> Property
        {
            get { return _property; }
            set
            {
                _property = value;
                RaisePropertyChanged("Property");
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

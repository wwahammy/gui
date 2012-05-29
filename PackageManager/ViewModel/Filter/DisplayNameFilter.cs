using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CoApp.Packaging.Common;
using GalaSoft.MvvmLight;
using CoApp.Toolkit.Linq;
using CTL = CoApp.Toolkit.Linq;

namespace CoApp.PackageManager.ViewModel.Filter
{
    
    /*


    public class DisplayNameFilterCreator : ObservableObject
    {
        private DisplayNameFilter _f;
       

        internal DisplayNameFilter Create(string beginningOfWord)
        {
            lock (this)
            {
                if (_f == null)
                {
                    var filter = new DisplayNameFilter();
                    _f = filter;
                }
                _f.FilterWords.Add(beginningOfWord);

                return _f;
            }
        }

        public void Delete(string beginningOfWord)
        {
            lock (this)
            {
                if (_f != null)
                {
                    _f.FilterWords.Remove(beginningOfWord);
                    if (!_f.FilterWords.Any())
                    {
                        _f = null;
                    }
                }
            }
        }

        public void DeleteEntireFilter(DisplayNameFilter filter)
        {
            _f = null;
        }


        public bool CanCreate
        {
            get { return true; }
        }
    }

    public class DisplayNameFilter : ObservableObject, IFilter
    {
        public Filter<IPackage> CalculatedFilter
        {
            get {
                if (FilterWords.Count == 0)
                {
                    return null;
                }
                var propExp = PropertyExpression<IPackage>.Create(p => p.DisplayName);
                Filter<IPackage> filter = CTL.Filter.Create(propExp, FilterOp.EQ, FilterWords[0]);
                filter = FilterWords.Skip(1).Aggregate(filter, (current, f) => current | CTL.Filter.Create(propExp, FilterOp.EQ, f));
                return filter;
            }
        }

        private ObservableCollection<string> _filterWords = new ObservableCollection<string>();

        public ObservableCollection<string> FilterWords
        {
            get { return _filterWords; }
            set
            {
                _filterWords = value;
                RaisePropertyChanged("FilterWords");
                RaisePropertyChanged("CalculatedFilter");
            }
        }


        public string FilterDescription
        {
            get { return "filter on " + 
                FilterWords.Aggregate(new StringBuilder(), (sb, input) => sb.Append(input + ", "));
            }
        }
    }*/
}

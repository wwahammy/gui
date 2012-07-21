using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CoApp.Packaging.Common;
using CoApp.Toolkit.Collections;
using CoApp.Toolkit.Linq;
using GalaSoft.MvvmLight.Command;

namespace CoApp.PackageManager.ViewModel.Filter
{
    public class FilterOnDateRange : GUIFilterBase
    {
        public FilterOnDateRange(FilterManagement management) : base(management)
        {
            ClearAfter = new RelayCommand(() => OnOrAfter = null);
            ClearBefore = new RelayCommand(() => OnOrBefore = null);
        }




        private DateTime? _onOrAfter;

        public DateTime? OnOrAfter
        {
            get { return _onOrAfter; }
            set
            {
                _onOrAfter = value;
                RaisePropertyChanged("OnOrAfter");
            }
        }


        private DateTime? _onOrBefore;

        public DateTime? OnOrBefore
        {
            get { return _onOrBefore; }
            set
            {
                _onOrBefore = value;
                RaisePropertyChanged("OnOrBefore");
            }
        }

        public ICommand ClearAfter { get; set; }
        public ICommand ClearBefore { get; set; }
        private readonly IDictionary<CAT, PropertyExpression<IPackage, DateTime>> CatToFilter = new XDictionary<CAT, PropertyExpression<IPackage, DateTime>>
                                                                              {
                                                                                  {
                                                                                      CAT.PublishDate,
                                                                                      PropertyExpression<IPackage>.
                                                                                        Create(p => p.PackageDetails.PublishDate)
                                                                                      },
                                                                              };

        public override FrictionlessFilter Create()
        {
            var filter = CatToFilter[Category];

            if (filter == null || OnOrAfter == null && OnOrBefore == null)
                return null;


            FrictionlessFilter f = null;
            Filter<IPackage> innerFilter = null;
            string filterDisplay = null;
            if (OnOrAfter == null && OnOrBefore != null)
            {
                innerFilter = filter.IsLessThanOrEqual(OnOrBefore.Value);
                filterDisplay = NiceName + " on or before " + OnOrBefore.Value;

            }
            else if (OnOrAfter != null && OnOrBefore == null)
            {
                innerFilter = filter.IsGreaterThanOrEqual(OnOrAfter.Value);
                filterDisplay = NiceName + " on or after " + OnOrAfter.Value;
            }
            else
            {
                innerFilter = filter.IsLessThanOrEqual(OnOrBefore.Value) & filter.IsGreaterThanOrEqual(OnOrAfter.Value);
                filterDisplay = NiceName + " on or after " + OnOrAfter.Value + " but on or before " + OnOrBefore.Value;
            }



            f = new FrictionlessFilter
                {
                    Category = Category,
                    Filter = innerFilter,
                    FilterDisplay = filterDisplay
                };

            OnOrAfter = null;
            OnOrBefore = null;
            return f;
        }

        public override bool CanBeCreated()
        {
            if (Management.AppliedFilters == null || !Management.AppliedFilters.Any())
                return true;

            return !(NumberOfFilter == NumOfFilter.Single && Management.AppliedFilters.Any(f => f.Category == Category));
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using CoApp.Packaging.Common;
using CoApp.Toolkit.Collections;
using CoApp.Toolkit.Linq;

namespace CoApp.PackageManager.ViewModel.Filter
{
    public class FilterOnBoolean : GUIFilterBase
    {
        private bool _input;

        public FilterOnBoolean(FilterManagement management) : base(management)
        {
        }

        public bool Input
        {
            get { return _input; }
            set
            {
                _input = value;
                RaisePropertyChanged("Input");
            }
        }


        private readonly IDictionary<CAT, PropertyExpression<IPackage, bool>> CatToFilter = new XDictionary<CAT, PropertyExpression<IPackage, bool>>
                                                                              {
                                                                                  {
                                                                                      CAT.IsBlocked,
                                                                                      PropertyExpression<IPackage>.
                                                                                        Create(p => p.IsBlocked)
                                                                                   }, {
                                                                                          CAT.IsInstalled,
                                                                                          PropertyExpression<IPackage>.Create(p => p.IsInstalled)
                                                                                          }, {
                                                                                                 CAT.IsDependency,
                                                                                                 PropertyExpression<IPackage>.Create(p => p.IsInstalled)
                                                                                                 },
                                                                                                 {
                                                                                                 CAT.IsWanted,
                                                                                                    PropertyExpression<IPackage>.Create(p => p.IsWanted)
                                                                                                    }
                                                                              };

        public override FrictionlessFilter Create()
        {
            var filter = CatToFilter[Category];
            if (filter == null)
                return null;
           
            return new FrictionlessFilter()
            {
                Category = Category,
                Filter = filter.Is(Input),
                FilterDisplay = NiceName + " is " + Input

            };
        }

        public override bool CanBeCreated()
        {
            if (Management.AppliedFilters == null || !Management.AppliedFilters.Any())
                return true;

            return !Management.AppliedFilters.Any(f => f.Category == Category);
        }
    }
}

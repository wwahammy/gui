using System.Collections.Generic;
using System.Linq;
using CoApp.Packaging.Common;
using CoApp.Toolkit.Collections;
using CoApp.Toolkit.Linq;

namespace CoApp.PackageManager.ViewModel.Filter
{
    public class FilterOnText : GUIFilterBase, IFilterWithTextInput, ISuperFilterCopy
    {
        private string _input;

        public FilterOnText(FilterManagement management) : base(management)
        {
        }

        public string Input
        {
            get { return _input; }
            set
            {
                _input = value;
                RaisePropertyChanged("Input");
            }
        }

        private readonly IDictionary<CAT, PropertyExpression<IPackage, string>> CatToFilter = new XDictionary<CAT, PropertyExpression<IPackage, string>>
                                                                              {
                                                                                  {
                                                                                      CAT.DisplayName,
                                                                                      PropertyExpression<IPackage>.
                                                                                        Create(p => p.DisplayName)
                                                                                      },
                                                                              };


        public override FrictionlessFilter Create()
        {

            var filter = CatToFilter[Category];

            if (filter == null && Category != CAT.FeedUrl)
                return null;

            FrictionlessFilter f = null;
        
            f = new FrictionlessFilter
                        {
                            Category = Category,
                            Filter = filter.Is(Input + "*"),
                            FilterDisplay = NiceName + " starts with " + Input
                        };

            Input = "";

            return f;

        }

        public override bool CanBeCreated()
        {
            if (Management.AppliedFilters == null || !Management.AppliedFilters.Any())
                return true;

            return ! (NumberOfFilter == NumOfFilter.Single && Management.AppliedFilters.Any(f => f.Category == Category));
        }

        public FrictionlessFilter CreateForSuperFilter(string input)
        {
            var filter = CatToFilter[Category];

            FrictionlessFilter f = new FrictionlessFilter
            {
                
                Filter = filter.Is(input + "*"),
                
            };

            return f;
        }

        public void FromInputSetValue(string input)
        {
            Input = input;
        }
    }
}

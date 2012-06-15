using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoApp.Packaging.Common;
using CoApp.Toolkit.Collections;
using CoApp.Toolkit.Linq;
using GalaSoft.MvvmLight;
using CTL = CoApp.Toolkit.Linq;

namespace CoApp.PackageManager.ViewModel.Filter
{
    class FilterOnText : GUIFilterBase
    {
        private string _input;

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
                                                                                      }
                                                                              };


        public override FrictionlessFilter Create()
        {
       
            var filter = CatToFilter[Category];
            
            if (filter == null)
                return null;



            var f = new FrictionlessFilter
                {
                    Category = Category,
                    Filter = filter.Is(Input + "*"),
                    FilterDisplay = NiceName + " starts with " + Input
                };

            Input = "";

            return f;

        }
    }
}

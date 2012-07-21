using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CoApp.Packaging.Common;
using CoApp.Toolkit.Linq;
using CoApp.Toolkit.Win32;
using CoApp.Gui.Toolkit.Support;
using CoApp.Updater.Support;

namespace CoApp.PackageManager.ViewModel.Filter
{
    public class FilterOnArchitecture : GUIFilterBase, IFilterOnChoices<Architecture>, ISuperFilterCopy
    {

        public FilterOnArchitecture(FilterManagement management) : base(management)
        {
            AllChoices = new ObservableCollection<KeyValuePair<Architecture, string>>();
            AllChoices.Add(new KeyValuePair<Architecture, string>(Architecture.Any, "Any"));

            if (Environment.Is64BitOperatingSystem)
                AllChoices.Add(new KeyValuePair<Architecture, string>(Architecture.x64, "64 bit"));

            AllChoices.Add(new KeyValuePair<Architecture, string>(Architecture.x86, "32 bit"));
        }

        

        public void FromInputSetValue(string input)
        {
            var kp = GetKpOrBust(input);
            if (kp != null)
            {
                Input = kp.Value;
            }
        }

        private KeyValuePair<Architecture,string> _input;

        public KeyValuePair<Architecture,string> Input
        {
            get { return _input; }
            set
            {
                _input = value;
                RaisePropertyChanged("Input");
            }
        }


       

        public ObservableCollection<KeyValuePair<Architecture, string>> AvailableChoices
        {
            get
            {
                return new ObservableCollection<KeyValuePair<Architecture, string>>(
                    AllChoices.Where(
                        f =>
                        !Management.AppliedFilters.OfType<FrictionlessFilter<Architecture>>().Any(
                            filt => filt.Category == Category && filt.ChoiceValue == f.Key)));

            }
        }


        public ObservableCollection<KeyValuePair<Architecture, string>> AllChoices { get; private set; }

        public override FrictionlessFilter Create()
        {
            return CreateFilter(Input);
        }

        public override bool CanBeCreated()
        {
            return AvailableChoices.Any();
        }

        public FrictionlessFilter CreateForSuperFilter(string input)
        {
            var kp = GetKpOrBust(input);
            if (kp == null)
            {
                return null;
            }

            return CreateFilter(kp.Value);
        }

        private FrictionlessFilter CreateFilter(KeyValuePair<Architecture, string> input)
        {
            var filter = PropertyExpression<IPackage>.Create(p => p.Architecture);


            return new FrictionlessFilter<Architecture>
            {
                ChoiceValue = input.Key,
                Category = Category,
                Filter = filter.Is(input.Key),
                FilterDisplay = NiceName + " is " + Input.Value

            };
        }

        private KeyValuePair<Architecture,string>? GetKpOrBust(string input)
        {
            var output = Architecture.Parse(input);

            try
            {
                var val = AllChoices.First(kp => kp.Key == output);
                return val;
            }
            catch
            {
                return null;
            }
        }
    }
}

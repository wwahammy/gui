using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CoApp.Toolkit.Extensions;
using CoApp.Toolkit.Win32;

namespace CoApp.PackageManager.ViewModel.Filter
{
    public class FilterOnFeedUrls : GUIFilterBase, IFilterWithTextInput, IFilterOnChoices<string>
    {
        private ObservableCollection<KeyValuePair<string, string>> _allChoices;

        public FilterOnFeedUrls(FilterManagement management) : base(management)
        {
        }

        public ObservableCollection<KeyValuePair<string, string>> AllChoices
        {
            get { return _allChoices; }
            set
            {
                _allChoices = value;
                RaisePropertyChanged("AllChoices");
                RaisePropertyChanged("AvailableChoices");
                

            }
        }
        
        private KeyValuePair<string, string> _input;

        public KeyValuePair<string, string> Input
        {
            get { return _input; }
            set
            {
                _input = value;
                RaisePropertyChanged("Input");
            }
        }
        
        

        public ObservableCollection<KeyValuePair<string, string>> AvailableChoices
        {
            get
            {   
                return new ObservableCollection<KeyValuePair<string, string>>(AllChoices.Where(
                        f => Management.AppliedFilters.OfType<FeedUrlFilter>().All(filt => filt.ChoiceValue != f.Key)));
            }
          
        }

        public override FrictionlessFilter Create()
        {
            var fuf = new FeedUrlFilter
                          {
                              Category = CAT.FeedUrl,
                              ChoiceValue = Input.Key,
                              FilterDisplay = NiceName + " is {0}".format(Input.Key)
                          };
            return fuf;
        }

        public override bool CanBeCreated()
        {
            return AvailableChoices.Any();
        }

        public void FromInputSetValue(string input)
        {
            var kp = GetKpOrBust(input);
            if (kp == null)
            {
                return;
            }

            Input = kp.Value;



        }


        private KeyValuePair<string, string>? GetKpOrBust(string input)
        {
            

            try
            {
                var val = AllChoices.First(kp => kp.Key == input);
                return val;
            }
            catch
            {
                return null;
            }
        }

       
    }
}

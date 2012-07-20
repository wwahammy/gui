using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoApp.PackageManager.ViewModel.Filter
{
    public interface IFilterOnChoices<TValue> : IFilterWithTextInput
    {
        KeyValuePair<TValue, string> Input { get; set; }

        ObservableCollection<KeyValuePair<TValue, string>> AvailableChoices { get; }

        ObservableCollection<KeyValuePair<TValue, string>> AllChoices { get; }
 
    }


   
}

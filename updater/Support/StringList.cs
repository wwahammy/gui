using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoApp.Updater.Support
{

    /// <summary>
    /// Ridiculous class needed because XAML is dumber than a box of rocks when it comes to generics
    /// </summary>
    public class StringList : ObservableCollection<string>
    {
    }
}

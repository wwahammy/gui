using System.Collections.ObjectModel;

namespace CoApp.Gui.Toolkit.Support
{

    /// <summary>
    /// Ridiculous class needed because XAML is dumber than a box of rocks when it comes to generics
    /// </summary>
    public class StringList : ObservableCollection<string>
    {
    }
}

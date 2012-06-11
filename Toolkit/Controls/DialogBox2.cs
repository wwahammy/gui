using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoApp.Gui.Toolkit.Controls
{
    public class DialogBox2 : ContentControl
    {
  
    }

    public class ButtonDescription
    {
        public string Title { get; set; }
        public bool IsCancel { get; set; }
        public ICommand Command { get; set; }
        public object CommandParameter { get; set; }
        public IInputElement CommandTarget { get; set; }
    }

    /// <summary>
    /// Created to make data templating slightly easier
    /// </summary>
    public class ElevateButtonDescription : ButtonDescription
    {
    }
}
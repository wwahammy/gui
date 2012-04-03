using System.Collections.ObjectModel;
using CoApp.Gui.Toolkit.Controls;

namespace CoApp.Gui.Toolkit.Messages
{
    public class MetroDialogBoxMessage
    {
        public string Title { get; set; }
        public object Content { get; set; }
        public ObservableCollection<ButtonDescription> Buttons { get; set; } 
    }
}

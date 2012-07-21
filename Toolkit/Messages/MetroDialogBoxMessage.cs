using System.Collections.ObjectModel;
using CoApp.Gui.Toolkit.Controls;
using CoApp.Gui.Toolkit.ViewModels;

namespace CoApp.Gui.Toolkit.Messages
{
    public class MetroDialogBoxMessage
    {
       
        public MetroDialogBoxMessage(ModalViewModel vm)
        {
            ModalViewModel = vm;
        }
        public ModalViewModel ModalViewModel { get; set; }
    }
}

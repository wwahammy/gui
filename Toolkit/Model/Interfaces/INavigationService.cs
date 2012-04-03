using System.Collections.ObjectModel;
using CoApp.Gui.Toolkit.ViewModels;

namespace CoApp.Gui.Toolkit.Model.Interfaces
{
    
    public interface INavigationService
    {

        void GoTo(ScreenViewModel viewModel);
        void GoTo(ScreenViewModel viewModel, bool saveInHistory);
        void Back();

        ScreenViewModel Current { get; }
        ReadOnlyCollection<ScreenViewModel> Stack { get; }

       
        
        bool StackEmpty { get; }

        
    }
}
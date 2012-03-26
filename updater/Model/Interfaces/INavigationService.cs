using System.Collections.ObjectModel;
using CoApp.Updater.Support;
using CoApp.Updater.ViewModel;
using GalaSoft.MvvmLight;

namespace CoApp.Updater.Model.Interfaces
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
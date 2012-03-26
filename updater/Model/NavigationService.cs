using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CoApp.Updater.Messages;
using CoApp.Updater.Model.Interfaces;
using CoApp.Updater.Support;
using CoApp.Updater.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace CoApp.Updater.Model
{
    public class NavigationService : INavigationService
    {
        private Stack<ScreenViewModel> _innerStack = new Stack<ScreenViewModel>();

        #region INavigationService Members


        private bool saveNextInHistory;
        
        public void GoTo(ScreenViewModel viewModel)
        {

           GoTo(viewModel, true);
        }

        public void GoTo(ScreenViewModel viewModel, bool saveInHistory)
        {
            if (Current != null)
            {
                Current.FireUnload();
                if (saveNextInHistory)
                    _innerStack.Push(Current);
            }
            saveNextInHistory = saveInHistory;
            Current = viewModel;
            Messenger.Default.Send(new GoToMessage { Destination = Current });
            Current.FireLoad();
        }

        public void Back()
        {
            Current.FireUnload();
             
            Current = _innerStack.Pop();
            
            Messenger.Default.Send(new GoToMessage {Destination = Current});
            Current.FireLoad();
        }

        public ScreenViewModel Current { get; private set; }


        public ReadOnlyCollection<ScreenViewModel> Stack
        {
            get { return new ReadOnlyCollection<ScreenViewModel>(_innerStack.ToArray()); }
        }

        /*
        public void ReloadStack(RestartInfo info)
        {
            var stack = new Stack<string>(info.TypeNames);

            var viewModelLocator = new ViewModelLocator();


            //This is the screen we ultimately go to
            string screenToGoTo = stack.Pop();

            //we reset the stack
            _innerStack = new Stack<ScreenViewModel>();
            foreach (string page in stack.Reverse())
            {
                _innerStack.Push(
                    (ScreenViewModel) viewModelLocator.GetType().GetProperty(page).GetValue(viewModelLocator, null));
            }
        }*/

        public bool StackEmpty
        {
            get { return _innerStack.Count < 1; }
        }

        internal void EmptyStack()
        {
            _innerStack = new Stack<ScreenViewModel>();
        }


        #endregion
    }
}
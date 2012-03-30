using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CoApp.Toolkit.Logging;
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


        private bool _saveNextInHistory;
        
        public void GoTo(ScreenViewModel viewModel)
        {
           
           GoTo(viewModel, true);
        }

        public void GoTo(ScreenViewModel viewModel, bool saveInHistory)
        {
            Logger.Message("Going to {0}, from {1}. Save in history? {2} " + Environment.NewLine, viewModel.GetType().Name, Current == null ? "null" : Current.GetType().Name, saveInHistory);
            if (Current != null)
            {
                Current.FireUnload();
                if (_saveNextInHistory)
                    _innerStack.Push(Current);
            }
            _saveNextInHistory = saveInHistory;
            Current = viewModel;
            Messenger.Default.Send(new GoToMessage { Destination = Current });
            Current.FireLoad();
        }

        public void Back()
        {
            Current.FireUnload();
            Logger.Message("Going back to {0} from {1}", _innerStack.Peek().GetType().Name, Current.GetType().Name);
            Current = _innerStack.Pop();
            
            Messenger.Default.Send(new GoToMessage {Destination = Current});
            Current.FireLoad();
        }

        public ScreenViewModel Current { get; private set; }


        public ReadOnlyCollection<ScreenViewModel> Stack
        {
            get { return new ReadOnlyCollection<ScreenViewModel>(_innerStack.ToArray()); }
        }

    
        public bool StackEmpty
        {
            get { return _innerStack.Count < 1; }
        }


        internal void Reset()
        {
            EmptyStack();
            ClearCurrent();
        }

        internal void EmptyStack()
        {
            _innerStack = new Stack<ScreenViewModel>();
        }

        internal void ClearCurrent()
        {
            Current = null;
        }


        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoApp.Updater.Model.Interfaces;

namespace CoApp.Updater.Model
{
    class InitializeService : IInitializeService
    {

        private Action _action;

        public bool Initialized { get; private set; }

        public void RunInitializationAction()
        {
            if (_action != null)
            {
                Initialized = true;
                _action.Invoke();
            }
        }

        public void SetInitializeAction(Action action)
        {
            _action = action;
        }
    }
}

using System;
using CoApp.Updater.Model.Interfaces;

namespace CoApp.Updater.Model
{
    public class AutomationService : IAutomationService
    {
        #region IAutomationService Members

        private bool _automatedInit = false;

        public bool IsAutomated { get; private set; }


        public void TurnOffAutomation()
        {
            if (IsAutomated)
            {
                IsAutomated = false;
                if (AutomationTurnedOff != null)
                {
                    AutomationTurnedOff();
                }
            }
        }

        public void TurnOnAutomation()
        {
            if (!_automatedInit)
            {
                IsAutomated = true;
                _automatedInit = true;
            }
        }

        public event Action AutomationTurnedOff;

        #endregion
    }
}
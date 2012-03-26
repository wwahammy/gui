using System;
using System.Collections.Generic;
using CoApp.Updater.ViewModel;

namespace CoApp.Updater.Model.Interfaces
{
    public interface IAutomationService
    {
        bool IsAutomated { get;}
        void TurnOffAutomation();
        void TurnOnAutomation();
        event Action AutomationTurnedOff;

        
    }
}
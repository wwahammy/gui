using System;
using CoApp.Gui.Toolkit.Model.Interfaces;
using GalaSoft.MvvmLight;

namespace CoApp.Gui.Toolkit.ViewModels.Settings
{
    [Serializable]
    
    public class PermissionViewModel : ViewModelBase
    {
        private bool? _canSelectUser;
        private bool? _everyonePermission;
        private PolicyResult _result;
        private bool? _userPermission;
        
        private bool? _oldValue = false;

        
        public bool? UserPermission
        {
            get { return _userPermission; }
            set
            {
                if (_userPermission != value)
                {
                    _userPermission = value;
                    RaisePropertyChanged("UserPermission");
                    HandleCheckedChange();
                }
            }
        }
        
        public bool? CanSelectUser
        {
            get { return _canSelectUser; }
            set
            {
                _canSelectUser = value;
                RaisePropertyChanged("CanSelectUser");
            }
        }





        
        public bool? EveryonePermission
        {
            get { return _everyonePermission; }
            set
            {
                if (_everyonePermission != value)
                {
                    _everyonePermission = value;

                    RaisePropertyChanged("EveryonePermission");
                    HandleCheckedChange();
                    if (value == true)
                    {
                        CanSelectUser = false;
                        _oldValue = UserPermission;
                        UserPermission = true;
                    }
                    else if (value == false)
                    {
                        CanSelectUser = true;
                        UserPermission = _oldValue;
                    }
                }
            }
        }

        
        public PolicyResult Result
        {
            get { return _result; }
            set
            {
              
                    _result = value;
                    RaisePropertyChanged("Result");
                    HandleResultChanged();
                
            }
        }

        private void HandleCheckedChange()
        {
            if (EveryonePermission == true)
            {
                Result = PolicyResult.Everyone;
            }
            else if (UserPermission == true)
            {
                Result = PolicyResult.CurrentUser;
            }
            else
            {
                Result = PolicyResult.Other;
            }
        }

        private void HandleResultChanged()
        {
            if (Result == PolicyResult.Everyone)
            {
                EveryonePermission = true;
            }
            else if (Result == PolicyResult.CurrentUser)
            {
                EveryonePermission = false;
                UserPermission = true;
            }

            else
            {
                EveryonePermission = false;
                UserPermission = false;
            }
        }
    }
}
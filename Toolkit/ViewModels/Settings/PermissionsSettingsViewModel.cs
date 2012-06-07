using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using CoApp.Gui.Toolkit.Messages;
using CoApp.Gui.Toolkit.Model;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Toolkit.Extensions;
using GalaSoft.MvvmLight.Command;

namespace CoApp.Gui.Toolkit.ViewModels.Settings
{
    [Serializable]
    
    public class PermissionsSettingsViewModel : ScreenViewModel
    {
        internal IPolicyService Policy;
        internal ICoAppService CoApp;

   
        private PermissionViewModel _changeSystemFeed;
        private PermissionViewModel _installing;
        private PermissionViewModel _remove;

        private PermissionViewModel _updating;

        private string _userName;

        public PermissionsSettingsViewModel()
        {
            Title = "Permissions";
            MessengerInstance.Register<PoliciesUpdatedMessage>(this, GetPoliciesAgain);
            Updating = new PermissionViewModel();
            Installing = new PermissionViewModel();
            Remove = new PermissionViewModel();
            //Active = new PermissionViewModel();
            //Block = new PermissionViewModel();
            //Require = new PermissionViewModel();
            SetState = new PermissionViewModel();
            
            ChangeSystemFeed = new PermissionViewModel();
            Policy = new LocalServiceLocator().PolicyService;
            CoApp = new LocalServiceLocator().CoAppService;
            Save = new RelayCommand(ExecuteSave);
            ElevateSave = new RelayCommand(ExecuteElevateSave);

            Loaded += OnLoaded;

           
            UserName = Policy.UserName;
        }

        private void ExecuteElevateSave()
        {
            CoApp.Elevate().Continue(() => Save.Execute(null));
        }

        protected override Task ReloadPolicies()
        {
          
            return _policyService.CanChangeSettings.ContinueWith(
                t => UpdateOnUI(() => CanChangeSettings = t.Result));
        }


        private void GetPoliciesAgain(PoliciesUpdatedMessage policiesUpdatedMessage = null)
        {
            ReloadPolicies();
            Policy.InstallPolicy.ContinueWith((t) =>
                                              UpdateOnUI(() => Installing.Result = t.Result));
            Policy.UpdatePolicy.ContinueWith((t) =>
                                             UpdateOnUI(() => Updating.Result = t.Result));

            Policy.RemovePolicy.ContinueWith((t) =>
                                             UpdateOnUI(() => Remove.Result = t.Result));
            Policy.SetStatePolicy.ContinueWith(t => 
                UpdateOnUI(() => SetState.Result = t.Result));
            

            Policy.SystemFeedsPolicy.ContinueWith(t => UpdateOnUI(() => ChangeSystemFeed.Result = t.Result));

          //  Policy.SessionFeedsPolicy.ContinueWith(t => UpdateOnUI(() => ChangeSessionFeed.Result = t.Result));
        }

        private void ExecuteSave()
        {
            var saveTasks = new []
                                {
                                    Policy.SetUpdatePolicy(Updating.Result),
                                    Policy.SetInstallPolicy(Installing.Result),
                                    Policy.SetRemovePolicy(Remove.Result),
                                    Policy.SetSetStatePolicy(SetState.Result),
                                    //Policy.SetSessionFeedsPolicy(ChangeSessionFeed.Result),
                                    Policy.SetSystemFeedsPolicy(ChangeSystemFeed.Result)
                                };
            //TODO: This should update the UI and mention it's been saved
            Task.Factory.ContinueWhenAll(saveTasks, (antecedents) => { });
        }

        
        public PermissionViewModel Updating
        {
            get { return _updating; }
            set
            {
                _updating = value;
                RaisePropertyChanged("Updating");
            }
        }

        
        public PermissionViewModel Installing
        {
            get { return _installing; }
            set
            {
                _installing = value;
                RaisePropertyChanged("Installing");
            }
        }

       
        public PermissionViewModel Remove
        {
            get { return _remove; }
            set
            {
                _remove = value;
                RaisePropertyChanged("Remove");
            }
        }

        private PermissionViewModel _setState;

        public PermissionViewModel SetState
        {
            get { return _setState; }
            set
            {
                _setState = value;
                RaisePropertyChanged("SetState");
            }
        }

        
        
        public PermissionViewModel ChangeSystemFeed
        {
            get { return _changeSystemFeed; }
            set
            {
                _changeSystemFeed = value;
                RaisePropertyChanged("ChangeSystemFeed");
            }
        }
        
    

        
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        private void OnLoaded()
        {
            GetPoliciesAgain();
        }

        
        
        public ICommand Save { get; set; }

        public ICommand ElevateSave { get; set; }
    }
}
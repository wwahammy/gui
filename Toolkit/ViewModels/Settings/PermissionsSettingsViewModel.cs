using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using CoApp.Gui.Toolkit.Model;
using CoApp.Gui.Toolkit.Model.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace CoApp.Gui.Toolkit.ViewModels.Settings
{
    [Serializable]
    
    public class PermissionsSettingsViewModel : ScreenViewModel
    {
        internal IPolicyService Policy;
        private PermissionViewModel _block;
        private PermissionViewModel _changeSessionFeed;
        private PermissionViewModel _changeSystemFeed;
        private PermissionViewModel _installing;
        private PermissionViewModel _remove;
        private PermissionViewModel _require;
        private PermissionViewModel _active;
        private PermissionViewModel _updating;

        private string _userName;

        public PermissionsSettingsViewModel()
        {
            Title = "Permissions";
            Updating = new PermissionViewModel();
            Installing = new PermissionViewModel();
            Remove = new PermissionViewModel();
            Active = new PermissionViewModel();
            Block = new PermissionViewModel();
            Require = new PermissionViewModel();
            ChangeSessionFeed = new PermissionViewModel();
            ChangeSystemFeed = new PermissionViewModel();
            

            Loaded += OnLoaded;

            Policy = new LocalServiceLocator().PolicyService;
            Save = new RelayCommand(ExecuteSave);

            UserName = Policy.UserName;
        }

        private void ExecuteSave()
        {
            var saveTasks = new Task[]
                                {
                                    Policy.SetUpdatePolicy(Updating.Result),
                                    Policy.SetInstallPolicy(Installing.Result),
                                    Policy.SetRemovePolicy(Remove.Result),
                                    Policy.SetBlockPolicy(Block.Result),
                                    Policy.SetRequirePolicy(Require.Result),
                                    Policy.SetActivePolicy(Active.Result),
                                    Policy.SetSessionFeedsPolicy(ChangeSessionFeed.Result),
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
        
        public PermissionViewModel Active
        {
            get { return _active; }
            set
            {
                _active = value;
                RaisePropertyChanged("Active");
            }
        }
        
        public PermissionViewModel Require
        {
            get { return _require; }
            set
            {
                _require = value;
                RaisePropertyChanged("Require");
            }
        }
        
        public PermissionViewModel Block
        {
            get { return _block; }
            set
            {
                _block = value;
                RaisePropertyChanged("Block");
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
        
        public PermissionViewModel ChangeSessionFeed
        {
            get { return _changeSessionFeed; }
            set
            {
                _changeSessionFeed = value;
                RaisePropertyChanged("ChangeSessionFeed");
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
            Policy.InstallPolicy.ContinueWith((t) =>
                                              UpdateOnUI(() => Installing.Result = t.Result));
            Policy.UpdatePolicy.ContinueWith((t) =>
                                             UpdateOnUI(() => Updating.Result = t.Result));

            Policy.RemovePolicy.ContinueWith((t) =>
                                             UpdateOnUI(() => Remove.Result = t.Result));
            Policy.ActivePolicy.ContinueWith(t => UpdateOnUI(() => Active.Result = t.Result));

            Policy.BlockPolicy.ContinueWith(t => UpdateOnUI(() => Block.Result = t.Result));

            Policy.RequirePolicy.ContinueWith(t => UpdateOnUI(() => Require.Result = t.Result));
            Policy.SystemFeedsPolicy.ContinueWith(t => UpdateOnUI(() => ChangeSystemFeed.Result = t.Result));

            Policy.SessionFeedsPolicy.ContinueWith(t => UpdateOnUI(() => ChangeSessionFeed.Result = t.Result));
        }
        
        public  ICommand Save { get; set; }

        public XElement Serialize()
        {
            return new XElement("PermissionsViewModel");
        }

        public  void Deserialize(XElement element)
        {
            
        }
    }
}
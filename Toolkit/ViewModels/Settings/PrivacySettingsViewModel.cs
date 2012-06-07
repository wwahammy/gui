using System.Windows.Input;
using System.Xml.Linq;
using CoApp.Gui.Toolkit.Model;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Toolkit.Extensions;
using CoApp.Toolkit.TaskService;
using GalaSoft.MvvmLight.Command;
using Task = System.Threading.Tasks.Task;

namespace CoApp.Gui.Toolkit.ViewModels.Settings
{
     
    public class PrivacySettingsViewModel : ScreenViewModel
    {
        internal ICoAppService CoApp;
         public PrivacySettingsViewModel()
         {
             Title = "Privacy";
             CoApp = new LocalServiceLocator().CoAppService;

             Loaded += OnLoaded;
             
             Save = new RelayCommand(OnSave );
             ElevateSave = new RelayCommand(OnElevateSave);
         }

        private void OnElevateSave()
        {
            var t = CoApp.Elevate();
            t.Continue(() => Save.Execute(null));
        }


        protected override Task ReloadPolicies()
         {
             return _policyService.CanChangeSettings.ContinueWith(t => UpdateOnUI(() => CanChangeSettings = t.Result));
         }

         private void OnLoaded()
         {
             ReloadPolicies().Wait();
             CoApp.OptedIn.ContinueWith((t) =>
                 UpdateOnUI(() => OptedIn = t.Result));
         }


        private void OnSave()
        {
            if (_optedIn != null)
            {
                CoApp.SetOptedIn((bool) _optedIn);

            }
        }

         private bool? _optedIn;

         public bool? OptedIn
         {
             get { return _optedIn; }
             set
             {
                 _optedIn = value;
                 RaisePropertyChanged("OptedIn");
                 RaisePropertyChanged("OptedOut");
             }
         }


  

         public bool? OptedOut
         {
             get { 
                 if (OptedIn != null)
                     {
                         return !(bool) OptedIn;
                     }
                 return OptedIn;
             }
            
         }

         
      
         public ICommand Save { get; set; }

         public ICommand ElevateSave { get; set; }
       
         
    }
}

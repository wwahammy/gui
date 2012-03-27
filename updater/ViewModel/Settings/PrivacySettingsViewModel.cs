using System;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml.Serialization;
using CoApp.Updater.Model;
using CoApp.Updater.Model.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace CoApp.Updater.ViewModel.Settings
{
     
    public class PrivacySettingsViewModel : ScreenViewModel
    {
        internal ICoAppService CoApp;
         public PrivacySettingsViewModel()
         {
             Title = "Privacy";
             CoApp = new LocalServiceLocator().CoAppService;

             Loaded += OnLoaded;
             
             Save = new RelayCommand(OnSave);
         }

         private void OnLoaded()
         {
             CoApp.OptedIn.ContinueWith((t) => UpdateOnUI(() => OptedIn = t.Result));
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


         public  XElement Serialize()
         {
             var root = new XElement("PrivacySettingsViewModel");
             root.SetAttributeValue("OptedIn", OptedIn);

             return root;
         }

         public void Deserialize(XElement element)
         {
             if (element.Name == "PrivacySettingsViewModel")
             {
                 XAttribute a = null;
                 if ((a = element.Attribute("OptedIn")) != null)
                 {
                     bool test;
                     if (bool.TryParse(a.Value, out test))
                     {
                         OptedIn = test;
                     }

                 }
             }
         }
    }
}

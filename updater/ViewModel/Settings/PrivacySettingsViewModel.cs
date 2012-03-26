using System;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml.Serialization;
using CoApp.Updater.Model;

namespace CoApp.Updater.ViewModel.Settings
{
     
    public class PrivacySettingsViewModel : ScreenViewModel
    {
         public PrivacySettingsViewModel()
         {
             Title = "Privacy";
             Loaded += OnLoaded;

         }

         private void OnLoaded()
         {
             new LocalServiceLocator().CoAppService.OptedIn.ContinueWith((t) => UpdateOnUI(() => OptedIn = t.Result));
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

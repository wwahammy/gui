using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.Updater.Support;

namespace CoApp.Updater.ViewModel.Errors
{
    public class UpdateFailureViewModel : ScreenViewModel
    {
        private UpdateFailureException _innerException;

        public UpdateFailureException InnerException
        {
            get { return _innerException; }
            set
            {
                _innerException = value;
                RaisePropertyChanged("InnerException");
            }
        }

        private ObservableCollection<Exception> _updateExceptions;

        public ObservableCollection<Exception> UpdateExceptions
        {
            get { return _updateExceptions; }
            set
            {
                _updateExceptions = value;
                RaisePropertyChanged("UpdateExceptions");
            }
        }

        

      

        public string ErrorTitle
        {
            get { 
                if (IsUpgrade)
                {
                    return "Upgrade failed";
                }
                return "Compatibility update failed";
            }
           
        }

    

        public string OriginalPackage
        {
            get { return InnerException.OriginalPackage; }
        }

     

        public string UpdatePackage
        {
            get { return InnerException.PackageToUpdateTo; }
        }

   

        public bool IsUpgrade
        {
            get { return InnerException.IsUpgrade; }
        }



        public string MessageOut
        {
            get { return ""; }

        }

        
        
    }
}

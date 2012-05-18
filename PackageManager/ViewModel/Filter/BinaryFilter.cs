using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;

namespace CoApp.PackageManager.ViewModel.Filter
{
    public class BinaryFilterCreator : ObservableObject
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        public bool CanCreate
        {
            get { return CanCreateTest(); }
          
        }

        Func<bool> CanCreateTest { get; set; }


       

        
    }


    public class BinaryFilter : ObservableObject
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        private bool _isSetToTrue;

        public bool IsSetToTrue
        {
            get { return _isSetToTrue; }
            set
            {
                _isSetToTrue = value;
                RaisePropertyChanged("IsSetToTrue");
            }
        }



        
        




        
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoApp.Toolkit.Exceptions;
using GalaSoft.MvvmLight;

namespace CoApp.Updater.Support.Errors
{
    public class ErrorObject : ObservableObject
    {
        public CoAppException Primary { get
        {
            if (Exceptions != null && Exceptions.Any())
            {
                return Exceptions.First();
            }
            return null;
        }
        }
        public IEnumerable<CoAppException> Exceptions { get; set; }
    }
}

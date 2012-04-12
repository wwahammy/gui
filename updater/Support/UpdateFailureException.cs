using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoApp.Toolkit.Engine.Client;
using CoApp.Toolkit.Exceptions;

namespace CoApp.Updater.Support
{
    public class UpdateFailureException : CoAppException
    {
     
        public UpdateFailureException(Exception innerException) : base("", innerException)
        {
            
        }
        public string OriginalPackage { get; set; }
        public string PackageToUpdateTo { get; set; }
        public bool IsUpgrade { get; set; }
        
    }
}

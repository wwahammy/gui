using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoApp.Packaging.Client;


namespace CoApp.PackageManager.Messages
{
    public class InstallationFailedMessage
    {
        public Package Package { get; set; }
        public Exception Exception { get; set; }
    }
}

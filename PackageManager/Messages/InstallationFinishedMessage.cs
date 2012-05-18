using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoApp.Packaging.Client;


namespace CoApp.PackageManager.Messages
{
    public class InstallationFinishedMessage
    {
        public Package Package { get; set; }
    }
}
